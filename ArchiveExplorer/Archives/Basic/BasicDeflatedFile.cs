using System.IO;

namespace Archive.Common
{
    public class BasicDeflatedFile : BasicFile
    {
        public override string Name { get; }
        public override IDevice Device { get; }
        public override IDirectory Parent => Device.Parent;

        public BasicArchive Archive { get; }

        public long Offset { get; }
        public long CompressedLength { get; }

        public override long Length { get; }
        public override FileLocation Location => Archive.Container.Location;
        public override bool Compressed => true;

        public BasicDeflatedFile(BasicArchive archive, string name, long offset, long length, long compressedLength)
        {
            Archive = archive;
            Name = name;
            Offset = offset;
            Length = length;
            CompressedLength = compressedLength;
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

            return new InflateStream(new OffsetStream(Archive.Container.Open(mode, access, share), Offset, CompressedLength), Length);
        }
    }
}