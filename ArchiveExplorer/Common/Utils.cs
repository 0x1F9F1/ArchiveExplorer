using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Archive.Common
{
    public static class Utils
    {
        public static bool CheckMagic(Stream stream, byte[] magic)
        {
            var data = new byte[magic.Length];

            stream.Read(data, 0, data.Length);

            return data.SequenceEqual(magic);
        }

        public static string ReadASCII(BinaryReader reader, int length)
        {
            var bytes = reader.ReadBytes(length);

            int terminator = Array.FindIndex(bytes, x => x == 0);

            if (terminator == -1)
            {
                terminator = bytes.Length;
            }

            return Encoding.ASCII.GetString(bytes, 0, terminator);
        }

        public static byte[] ReadAllBytes(Stream stream)
        {
#if false
        using (var ms = new MemoryStream())
        {
            var buffer = new byte[8192];

            for (int read = 0; (read = stream.Read(buffer, 0, buffer.Length)) > 0;)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
#else
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                return ms.ToArray();
            }
#endif
        }
    }
}
