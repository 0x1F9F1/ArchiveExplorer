using System.IO;

namespace Archive
{
    public interface IFile : INode
    {
        long Length { get; }

        // TODO: Additional Data:
        // Attributes (Read/Write)
        // Date Created/Modified/Accessed
        // Length on Disk (aka Compressed Size)

        Stream Open(FileMode mode, FileAccess access, FileShare share);

        Stream OpenRead();
        Stream OpenWrite();

        void CopyTo(IFile dest);
    }
}