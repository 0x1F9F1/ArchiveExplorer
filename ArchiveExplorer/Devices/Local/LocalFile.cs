using System.IO;

namespace Archive.Local
{
    public class LocalFile : BasicFile
    {
        protected FileInfo info_;

        public override string Name => info_.Name;
        public override IDevice Device { get; }
        public override IDirectory Parent => new LocalDirectory(info_.Directory, Device);

        public override long Length
        {
            get
            {
                info_.Refresh();

                return info_.Length;
            }
        }

        public LocalFile(FileInfo info, IDevice device)
        {
            info_ = info;
            Device = device;
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return info_.Open(mode, access, share);
        }
    }
}