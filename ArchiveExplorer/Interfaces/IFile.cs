using System.IO;

namespace Archive
{
    public enum FileLocation
    {
        Local,
        Remote,
        Virtual
    }

    public interface IFile : INode
    {
        long Length { get; }
        FileLocation Location { get; }
        bool Compressed { get; }

        // TODO: Additional Data:
        // Attributes (Read/Write)
        // Date Created/Modified/Accessed

        Stream Open(FileMode mode, FileAccess access, FileShare share);

        Stream OpenRead();
        Stream OpenWrite();

        void CopyTo(IFile dest);
    }
}