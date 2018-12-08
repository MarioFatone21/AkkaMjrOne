using System.IO;
using System.Text;
using Akka.Actor;

namespace AkkaMjrOne.Step6.Mono.Completed
{
    public class TailActor : UntypedActor
    {
        #region Message types

        /// <summary>
        /// Signal that the file has changed, and we need to read the next line of the file.
        /// </summary>
        public class FileWrite
        {
            public FileWrite(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; private set; }
        }

        /// <summary>
        /// Signal that the OS had an error accessing the file.
        /// </summary>
        public class FileError
        {
            public FileError(string fileName, string reason)
            {
                FileName = fileName;
                Reason = reason;
            }

            public string FileName { get; private set; }

            public string Reason { get; private set; }
        }

        /// <summary>
        /// Signal to read the initial contents of the file at actor startup.
        /// </summary>
        public class InitialRead
        {
            public InitialRead(string fileName, string text)
            {
                FileName = fileName;
                Text = text;
            }

            public string FileName { get; private set; }
            public string Text { get; private set; }
        }

        #endregion

        private readonly string _filePath;
        private readonly IActorRef _reporterActor;

        private FileObserver _observer;
        private string lastReadText;

        public TailActor(IActorRef reporterActor, string filePath)
        {
            _reporterActor = reporterActor;
            _filePath = filePath;
        }

        /// <summary>
        /// Initialization logic for actor that will tail changes to a file.
        /// </summary>
        protected override void PreStart()
        {
            // start watching file for changes
            _observer = new FileObserver(Self, Path.GetFullPath(_filePath));
            _observer.Start();

            // read the initial contents of the file and send it to console as first message
            var text = lastReadText = ReadFile();
            Self.Tell(new InitialRead(_filePath, text));
        }

        /// <summary>
        /// Cleanup OS handles for <see cref="FileObserver"/>.
        /// </summary>
        protected override void PostStop()
        {
            _observer.Dispose();
            _observer = null;
            base.PostStop();
        }

        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                // this is assuming a log file type format that is append-only
                var text = ReadFile();
                if (!string.IsNullOrEmpty(text) && !lastReadText.Equals(text) && lastReadText.Length < text.Length)
                {
                    var diff = text.Substring(lastReadText.Length, (text.Length - lastReadText.Length));

                    _reporterActor.Tell(diff);
                }
            }
            else if (message is FileError)
            {
                var fe = message as FileError;
                _reporterActor.Tell(string.Format("Tail error: {0}", fe.Reason));
            }
            else if (message is InitialRead)
            {
                var ir = message as InitialRead;
                _reporterActor.Tell(ir.Text);
            }
        }

        private string ReadFile()
        {
            var text = string.Empty;

            // open the file stream with shared read/write permissions (so file can be written to while open)
            using (var fileStream = new FileStream(Path.GetFullPath(_filePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var fileStreamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = fileStreamReader.ReadToEnd();

            }
            return text;
        }
    }
}
