namespace Archive
{
    public interface INode
    {
        string Name { get; }

        IDevice Device { get; }
        IDirectory Parent { get; }
    }
}
