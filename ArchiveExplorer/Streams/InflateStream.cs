using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive
{
    class InflateStream : DeflateStream
    {
        public override long Length { get; }

        protected static int CalculateBufferSize(long compressedLength)
        {
            // TODO: Adjust buffer size based on comprsesed length

            return 1024 * 1024 * 64; // 64 MB
        }

        public InflateStream(Stream stream, long offset, long length, long compressedLength)
            : base(new OffsetStream(new BufferedStream(stream, CalculateBufferSize(compressedLength)), offset, compressedLength), CompressionMode.Decompress)
        {
            Length = length;
        }
    }
}
