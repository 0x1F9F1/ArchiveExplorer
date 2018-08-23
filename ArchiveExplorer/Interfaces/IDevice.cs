namespace Archive
{
    public interface IDevice : IDirectory
    {
        IDevice QueryPath(string path);
    }
}
