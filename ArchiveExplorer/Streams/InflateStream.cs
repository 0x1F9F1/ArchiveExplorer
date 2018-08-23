using System.IO;
using System.IO.Compression;

namespace Archive
{
    class InflateStream : DeflateStream
    {
        public override long Length { get; }

        protected static int CalculateBufferSize(long compressedLength)
        {
            // TODO: Adjust buffer size based on comprsesed length
            // TODO: Check if input stream can seek?

            int result = 1024 * 1024 * 64; // 64 MB

            if (result > compressedLength)
            {
                result = (int) compressedLength;
            }

            return result;
        }

        public InflateStream(Stream stream, long length)
            : base(new BufferedStream(stream, CalculateBufferSize(stream.Length)), CompressionMode.Decompress)
        {
            Length = length;
        }
    }
}
