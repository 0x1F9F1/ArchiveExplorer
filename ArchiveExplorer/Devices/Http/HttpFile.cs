using System;
using System.IO;

namespace Archive.Web
{
    public class HttpFile : BasicFile
    {
        protected readonly Uri path_;
        protected readonly Lazy<long> length_;

        public override long Length => length_.Value;
        public override string Name => Path.GetFileName(path_.AbsolutePath);
        public override IDevice Device { get; }
        public override IDirectory Parent => null;

        public HttpFile(Uri path, HttpDevice device)
        {
            path_ = path;
            Device = device;

            length_ = new Lazy<long>(() =>
            {
                using (var stream = OpenRead())
                {
                    return stream.Length;
                }
            });
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            if (mode != FileMode.Open)
            {
                return null;
            }

            if (access != FileAccess.Read)
            {
                return null;
            }

            return new BufferedStream(new HttpStream(path_));
        }
    }
}
