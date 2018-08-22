using System.IO;
using System.Text.RegularExpressions;

namespace Archive.Local
{
    public class LocalDevice : IDevice
    {
        public string Name => "Local File System";
        public IDevice Device => this;
        public IDirectory Parent => null;

        protected static Regex LocalPathRegex = new Regex(@"^[a-zA-Z]:[\\\/]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IDevice QueryDevice(string path)
        {
            if (LocalPathRegex.IsMatch(path))
            {
                return this;
            }

            return null;
        }

        public IDirectory GetDirectory(string path)
        {
            var info = new DirectoryInfo(path);

            return new LocalDirectory(info, this);
        }

        public IFile GetFile(string path)
        {
            var info = new FileInfo(path);

            return new LocalFile(info, this);
        }
    }
}