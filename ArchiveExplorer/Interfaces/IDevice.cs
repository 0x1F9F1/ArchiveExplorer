namespace Archive
{
    public interface IDevice : IDirectory
    {
        IDevice QueryDevice(string path);
    }
}
