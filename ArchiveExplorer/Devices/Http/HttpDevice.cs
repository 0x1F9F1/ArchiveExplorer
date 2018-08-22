using System;

namespace Archive.Web
{
    public class HttpDevice : IDevice
    {
        public string Name => "HTTP Device";
        public IDevice Device => this;
        public IDirectory Parent => null;

        protected static Uri ParsePath(string path)
        {
            if (Uri.TryCreate(path, UriKind.Absolute, out Uri uri))
            {
                if (IsUriSupported(uri))
                {
                    return uri;
                }
            }

            return null;
        }

        protected static bool IsUriSupported(Uri uri)
        {
            return (uri.Scheme == Uri.UriSchemeHttp)
                || (uri.Scheme == Uri.UriSchemeHttps);
        }

        public IDevice QueryDevice(string path)
        {
            var uri = ParsePath(path);

            if (uri != null)
            {
                return this;
            }

            return null;
        }

        public IDirectory GetDirectory(string path)
        {
            throw new NotSupportedException();
        }

        public IFile GetFile(string path)
        {
            var uri = ParsePath(path);

            if (uri != null)
            {
                return new HttpFile(uri, this);
            }

            return null;
        }
    }
}
