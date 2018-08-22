using System.Collections.Generic;

namespace Archive.Root
{
    public class RootDevice : IDevice
    {
        public string Name => "ROOT Device";
        public IDevice Device => this;
        public IDirectory Parent => null;

        protected List<IDevice> roots_ = new List<IDevice>();

        public IDevice QueryDevice(string path)
        {
            foreach (var root in roots_)
            {
                var device = root.QueryDevice(path);

                if (device != null)
                {
                    return device;
                }
            }

            return null;
        }

        public IDirectory GetDirectory(string path)
        {
            var device = QueryDevice(path);

            return device?.GetDirectory(path);
        }

        public IFile GetFile(string path)
        {
            var device = QueryDevice(path);

            return device?.GetFile(path);
        }

        public void AddDevice(IDevice device)
        {
            roots_.Add(device);
        }
    }
}
