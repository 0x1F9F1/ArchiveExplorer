using System;
using System.IO;
using System.Net;

namespace Archive.Web
{
    public class HttpStream : Stream
    {
        protected readonly Uri path_;
        protected long length_;
        protected long position_;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => length_;

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
                path_ = response.ResponseUri;
                length_ = response.ContentLength;
            }

            position_ = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var request = (HttpWebRequest)WebRequest.Create(path_);

            request.Method = "GET";

            if (position_ == 0)
            {
                request.AddRange(count);
            }
            else
            {
                request.AddRange(position_, position_ + count);
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
                    offset += length_;
                    break;
            }

            if (offset > length_)
            {
                offset = length_;
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
            length_ = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var request = (HttpWebRequest)WebRequest.Create(path_);

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