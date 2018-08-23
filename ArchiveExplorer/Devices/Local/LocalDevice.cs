using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Archive.Local
{
    public class LocalDevice : IDevice
    {
        public string Name => "Local File System";
        public IDevice Device => this;
        public IDirectory Parent => null;

        public ICollection<IFile> Files => throw new NotSupportedException();
        public ICollection<IDirectory> Directories => throw new NotSupportedException();

        protected static Regex LocalPathRegex = new Regex(@"^[a-zA-Z]:[\\\/]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IDevice QueryPath(string path)
        {
            if (LocalPathRegex.IsMatch(path))
            {
                return this;
            }

            return null;
        }

        public IDirectory GetDirectory(string path)
        {
            var info = new DirectoryInfo(path);

            return new LocalDirectory(info, this);
        }

        public IFile GetFile(string path)
        {
            var info = new FileInfo(path);

            return new LocalFile(info, this);
        }

        public bool RemoveFile(string name)
        {
            throw new NotImplementedException();
        }

        public bool RemoveDirectory(string name)
        {
            throw new NotImplementedException();
        }
    }
}