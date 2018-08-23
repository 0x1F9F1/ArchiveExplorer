using System;
using System.Collections.Generic;
using System.IO;

namespace Archive.Common
{
    public class BasicArchive : IArchive
    {
        // TODO: Should this be a tree structure?
        protected readonly Dictionary<string, IFile> files_ = new Dictionary<string, IFile>();

        public IFile Container { get; protected set; }

        public string Name => Container.Name;
        public IDevice Device => Container.Device;
        public IDirectory Parent => Container.Parent;

        // TODO: Implement archive directories
        public ICollection<IFile> Files => files_.Values;

        // TODO: Implement archive directories
        public ICollection<IDirectory> Directories => throw new NotImplementedException();

        public BasicArchive(IFile container, bool caseSensitive)
        {
            Container = container;

            files_ = new Dictionary<string, IFile>(caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
        }

        public IDirectory GetDirectory(string path)
        {
            // TODO: Implement archive directories

            throw new NotImplementedException();
        }

        public IFile GetFile(string path)
        {
            if (files_.TryGetValue(path, out IFile file))
            {
                return file;
            }

            return null;
        }

        public void AddFile(string path, IFile file)
        {
            files_.Add(path, file);
        }

        public void AddStoredFile(string path, long offset, long length)
        {
            AddFile(path, new BasicStoredFile(this, Path.GetFileName(path), offset, length));
        }

        public void AddDeflatedFile(string path, long offset, long length, long compressedLength)
        {
            AddFile(path, new BasicDeflatedFile(this, Path.GetFileName(path), offset, length, compressedLength));
        }

        public void Preload()
        {
            // TODO: Replace Container with a version loaded completely into memory.
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