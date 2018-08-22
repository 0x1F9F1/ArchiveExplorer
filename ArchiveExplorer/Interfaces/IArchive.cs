namespace Archive
{
    public interface IArchive : IDirectory
    {
        IFile Container { get; }

        void Preload();
    }
}
