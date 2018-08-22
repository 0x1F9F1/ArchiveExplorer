using System;
using System.Text;
using System.IO;

namespace Archive.Common
{
    public class ZipArchiveType : IArchiveType
    {
        public string Name => "Zip (PK) Archive";
        public string Identifier => "ZIP";

        protected static byte[] ZIPENDLOCATOR_MAGIC = new byte[] { 0x50, 0x4B, 0x05, 0x06 }; // PK\x05\x06

        public bool IsValid(IFile file, Stream stream)
        {
            if (stream.Length < 22)
            {
                return false;
            }

            stream.Seek(-22, SeekOrigin.End);

            return Utils.CheckMagic(stream, ZIPENDLOCATOR_MAGIC);
        }

        public IArchive GetArchive(IFile file, Stream stream)
        {
            if (stream.Length < 22)
            {
                return null;
            }

            stream.Seek(-22, SeekOrigin.End);

            if (!Utils.CheckMagic(stream, ZIPENDLOCATOR_MAGIC))
            {
                throw new Exception("Invalid Archive");
            }

            var reader = new BinaryReader(stream, Encoding.ASCII, true);

            var diskNumber = reader.ReadUInt16();
            var startDiskNumber = reader.ReadUInt16();

            if (diskNumber != startDiskNumber)
            {
                throw new Exception("Incomplete Archive");
            }

            var fileCount = reader.ReadUInt16();
            var filesInDirectory = reader.ReadUInt16();

            var directorySize = reader.ReadUInt32();
            var directoryOffset = reader.ReadUInt32();

            var fileCommentLength = reader.ReadUInt16();

            var currentOffset = directoryOffset;

            var result = new BasicArchive(file, true);

            while (true)
            {
                stream.Position = currentOffset;

                if (reader.ReadUInt32() != 0x02014B50) // ZIPDIRENTRY
                {
                    break;
                }

                var versionMadeBy = reader.ReadUInt16();
                var versionToExtract = reader.ReadUInt16();
                var flags = reader.ReadUInt16();

                var compressionMethod = reader.ReadUInt16();

                var fileTime = reader.ReadUInt16();
                var fileDate = reader.ReadUInt16();
                var crc = reader.ReadUInt32();

                var compressedSize = reader.ReadUInt32();
                var uncompressedSize = reader.ReadUInt32();

                var nameLength = reader.ReadUInt16();
                var extraLength = reader.ReadUInt16();
                var commentLength = reader.ReadUInt16();

                var diskNumberStart = reader.ReadUInt16();

                var internalAttributes = reader.ReadUInt16();
                var externalAttributes = reader.ReadUInt32();

                var recordOffset = reader.ReadUInt32(); // ZIPFILERECORD

                var fullName = Utils.ReadASCII(reader, nameLength);

                currentOffset = (uint)stream.Position + extraLength + commentLength;

                var dataOffset = recordOffset + 30 + nameLength;

                if (fullName.EndsWith("/"))
                {
                    continue;
                }

                if (compressionMethod == 0)
                {
                    result.AddStoredFile(fullName, dataOffset, uncompressedSize);
                }
                else if (compressionMethod == 8)
                {
                    result.AddDeflatedFile(fullName, dataOffset, uncompressedSize, compressedSize);
                }
                else
                {
                    throw new Exception($"Invalid compression method: {compressionMethod} - expected STORE (0) or DEFLATE (8)");
                }
            }

            return result;
        }
    }
}