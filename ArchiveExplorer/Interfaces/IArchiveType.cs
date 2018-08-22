using System.IO;

namespace Archive
{
    public interface IArchiveType
    {
        string Name { get; }
        string Identifier { get; }

        bool IsValid(IFile file, Stream stream);
        IArchive GetArchive(IFile file, Stream stream);

        // TODO: Save Archive
    }
}
