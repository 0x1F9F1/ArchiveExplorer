using System;
using System.IO;

namespace Archive
{
    public class OffsetStream : Stream
    {
        protected readonly Stream parent_;
        protected readonly long start_;
        protected readonly long length_;
        protected long position_;

        public override bool CanRead => parent_.CanRead;
        public override bool CanSeek => parent_.CanSeek;
        public override bool CanWrite => false;

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

        public override long Length
        {
            get
            {
                return length_;
            }
        }

        public OffsetStream(Stream parent, long offset, long length)
        {
            parent_ = parent;
            start_ = offset;
            length_ = length;
            position_ = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var position = start_ + position_;

            if (parent_.Position != position)
            {
                if (parent_.CanSeek)
                {
                    parent_.Seek(position, SeekOrigin.Begin);
                }
                else
                {
                    throw new NotSupportedException("Cannot Seek");
                }
            }

            if (count + position_ > length_)
            {
                count = (int)(length_ - position_);
            }

            int bytesRead = parent_.Read(buffer, offset, count);

            position_ += bytesRead;

            return bytesRead;
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
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}