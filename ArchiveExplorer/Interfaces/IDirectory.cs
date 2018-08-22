namespace Archive
{
    public interface IDirectory : INode
    {
        IFile GetFile(string name);
        IDirectory GetDirectory(string name);

        // TODO: Add/Remove/Enumerate File/Directory 

        // TODO: Directory Statistics:
        // Date Created/Modified/Accessed
        // Attributes (Read/Write)
        // File/Directory Count
        // Total Files Size
    }
}
