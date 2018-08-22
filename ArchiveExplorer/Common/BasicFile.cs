using System.IO;

namespace Archive
{
    public abstract class BasicFile : IFile
    {
        public abstract long Length { get; }
        public abstract string Name { get; }
        public abstract IDevice Device { get; }
        public abstract IDirectory Parent { get; }

        public abstract Stream Open(FileMode mode, FileAccess access, FileShare share);

        public virtual Stream OpenRead() => Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        public virtual Stream OpenWrite() => Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

        public virtual void CopyTo(IFile dest)
        {
            using (var input = OpenRead())
            using (var output = dest.OpenWrite())
            {
                input.CopyTo(output);
            }
        }
    }
}
