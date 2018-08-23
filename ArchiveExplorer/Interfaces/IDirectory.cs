using System.Collections.Generic;

namespace Archive
{
    public interface IDirectory : INode
    {
        ICollection<IFile> Files { get; }
        ICollection<IDirectory> Directories { get; }

        IFile GetFile(string name);
        IDirectory GetDirectory(string name);

        bool RemoveFile(string name);
        bool RemoveDirectory(string name);

        // TODO: Directory Statistics:
        // Date Created/Modified/Accessed
        // Attributes (Read/Write)
        // File/Directory Count
        // Total Files Size
    }
}
