using System;
using System.Diagnostics;

namespace AkkaMjrOne.Step6.Mono
{
    internal struct FileState
    {
        internal long _version;  // removal notification are implemented something similar to "mark and sweep". This value is incremented in the mark phase
        public string Path;
        public string Directory;
        public DateTimeOffset LastWriteTimeUtc;
        public long Length;

        public FileState(string directory, string path) : this()
        {
            Debug.Assert(path != null);
            Directory = directory;
            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
