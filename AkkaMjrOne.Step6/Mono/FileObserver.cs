using System;
using System.IO;
using Akka.Actor;

namespace AkkaMjrOne.Step6.Mono
{
    /// <summary>
    /// Turns <see cref="FileSystemWatcher"/> events about a specific file into messages for <see cref="TailActor"/>.
    /// </summary>
    public class FileObserver : IDisposable
    {
        private PollingFileSystemWatcher _watcher;
        private readonly IActorRef _tailActor;
        private readonly string _absoluteFilePath;
        private readonly string _fileDir;
        private readonly string _fileExtension;
        private readonly string _fileNameOnly;

        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            _tailActor = tailActor;
            _absoluteFilePath = absoluteFilePath;
            _fileDir = Path.GetDirectoryName(absoluteFilePath);
            _fileExtension = Path.GetExtension(absoluteFilePath);
            _fileNameOnly = Path.GetFileName(absoluteFilePath);
        }

        /// <summary>
        /// Begin monitoring file.
        /// </summary>
        public void Start()
        {
            // Need this for Mono 3.12.0 workaround
            // uncomment this line if you're running on Mono!
            Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");

            // make watcher to observe our specific file
            _watcher = new PollingFileSystemWatcher(_fileDir, "*" + _fileExtension);

            // assign callbacks for event types
            _watcher.ChangedDetailed += OnFileChanged;
            _watcher.Error += OnFileError;

            // start watching
            _watcher.Start();
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }

        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file error events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFileError(object sender, ErrorEventArgs e)
        {
            // notify the tail actor a file error has occured
            // TODO
            _tailActor.Tell(new TailActor.FileError(_fileNameOnly, e.GetException().Message), ActorRefs.NoSender);
        }

        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file change events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFileChanged(object sender, PollingFileSystemEventArgs e)
        {
            foreach (var change in e.Changes)
            {
                if (change.ChangeType == WatcherChangeTypes.Changed)
                {
                    // here we use a special ActorRefs.NoSender
                    // since this event can happen many times, this is a little microoptimization
                    _tailActor.Tell(new TailActor.FileWrite(change.Name), ActorRefs.NoSender);
                    break;
                }
            }

        }
    }
}
