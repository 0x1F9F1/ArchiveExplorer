using System;
using System.IO;
using System.Net;

namespace Archive.Web
{
    public class HttpStream : Stream
    {
        protected long position_;

        public Uri Path { get; }

        public override bool CanRead => true;
        public override bool CanSeek { get; }
        public override bool CanWrite => false;
        public override long Length { get; }

        public override long Position
        {
            get
            {
                return position_;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        public HttpStream(Uri path)
        {
            var request = (HttpWebRequest)WebRequest.Create(path);

            request.Method = "HEAD";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Path = response.ResponseUri;
                Length = response.ContentLength;
                CanSeek = response.Headers["Accept-Ranges"]?.Equals("bytes") ?? false;
            }

            position_ = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count > 0)
            {
                var request = (HttpWebRequest)WebRequest.Create(Path);

                request.Method = "GET";

                if (position_ == 0)
                {
                    request.AddRange(count);
                }
                else
                {
                    request.AddRange(position_, position_ + count - 1);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        int total = 0;

                        while ((total < count) && (total < response.ContentLength))
                        {
                            int read = stream.Read(buffer, offset + total, count - total);

                            if (read > 0)
                            {
                                total += read;
                            }
                            else
                            {
                                break;
                            }
                        }

                        position_ += total;

                        return total;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    break;

                case SeekOrigin.Current:
                    offset += position_;
                    break;

                case SeekOrigin.End:
                    offset += Length;
                    break;
            }

            if (offset > Length)
            {
                offset = Length;
            }
            else if (offset < 0)
            {
                offset = 0;
            }

            position_ = offset;

            return position_;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var request = (HttpWebRequest)WebRequest.Create(Path);

            request.Method = "PUT";
            request.ContentLength = count;
            request.AllowWriteStreamBuffering = true;
            request.AddRange(position_, position_ + count);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(buffer, offset, count);
            }

            var response = (HttpWebResponse)request.GetResponse();
        }

        public override void Flush()
        {

        }
    }
}