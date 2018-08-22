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
    }
}
