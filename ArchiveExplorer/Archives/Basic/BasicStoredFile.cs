using System.IO;

namespace Archive.Common
{
    public class BasicStoredFile : BasicFile
    {
        public override string Name { get; }
        public override IDevice Device { get; }
        public override IDirectory Parent => Device.Parent;

        public BasicArchive Archive { get; }

        public long Offset { get; }

        public override long Length { get; }
        public override FileLocation Location => Archive.Container.Location;
        public override bool Compressed => Archive.Container.Compressed;

        public BasicStoredFile(BasicArchive archive, string name, long offset, long length)
        {
            Archive = archive;
            Name = name;
            Offset = offset;
            Length = length;
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

            return new OffsetStream(Archive.Container.Open(mode, access, share), Offset, Length);
        }
    }
}