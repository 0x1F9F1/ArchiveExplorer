﻿using System.IO;

namespace Archive.Local
{
    public class LocalDirectory : IDirectory
    {
        protected DirectoryInfo info_;

        public string Name => info_.Name;

        public IDevice Device { get; }

        public IDirectory Parent => new LocalDirectory(info_.Parent, Device);

        public LocalDirectory(DirectoryInfo info, IDevice device)
        {
            info_ = info;
            Device = device;
        }

        public IDirectory GetDirectory(string name)
        {
            var results = info_.GetDirectories(name, SearchOption.TopDirectoryOnly);

            if (results.Length == 1)
            {
                return new LocalDirectory(results[0], Device);
            }

            return null;
        }

        public IFile GetFile(string name)
        {
            var results = info_.GetFiles(name, SearchOption.TopDirectoryOnly);

            if (results.Length == 1)
            {
                return new LocalFile(results[0], Device);
            }

            return null;
        }
    }
}