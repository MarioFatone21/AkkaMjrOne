using System.Diagnostics;
using System.IO;

namespace AkkaMjrOne.Step6.Mono
{
    public struct FileChange
    {
        internal FileChange(string directory, string path, WatcherChangeTypes type)
        {
            Debug.Assert(path != null);
            Directory = directory;
            Name = path;
            ChangeType = type;
        }

        public string Directory { get; }
        public string Name { get; }
        public WatcherChangeTypes ChangeType { get; }
    }
}
