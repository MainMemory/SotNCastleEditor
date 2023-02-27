using System;
using System.IO;

namespace SotNData
{
	public class DiscImage2352Stream : Stream
	{
		public Stream BaseStream { get; }

		public override bool CanRead => BaseStream.CanRead;

		public override bool CanSeek => BaseStream.CanSeek;

		public override bool CanWrite => BaseStream.CanWrite;

		public override bool CanTimeout => BaseStream.CanTimeout;

		public override int ReadTimeout { get => BaseStream.ReadTimeout; set => BaseStream.ReadTimeout = value; }

		public override int WriteTimeout { get => BaseStream.WriteTimeout; set => BaseStream.WriteTimeout = value; }

		public override long Length => BaseStream.Length / 2352 * 2048;

		public override long Position
		{
			get => BaseStream.Position - (BaseStream.Position / 2352 * 304) - 0x18;
			set => BaseStream.Position = value + (value / 2048 * 304) + 0x18;
		}

		public DiscImage2352Stream(Stream baseStream)
		{
			BaseStream = baseStream;
			Position = Math.Max(Position, 0);
		}

		public override void Flush() => BaseStream.Flush();

		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesRead = 0;
			while (count > 0)
			{
				int sectnct = 2048 - (int)(Position % 2048);
				int bytesToRead = Math.Min(sectnct, count);
				var tmp = BaseStream.Read(buffer, offset, bytesToRead);
				count -= tmp;
				bytesRead += tmp;
				if (tmp < bytesToRead)
					return bytesRead;
				offset += tmp;
				if (tmp == sectnct)
					BaseStream.Seek(304, SeekOrigin.Current);
			}
			return bytesRead;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
					return Position = offset;
				case SeekOrigin.Current:
					return Position += offset;
				case SeekOrigin.End:
					return Position = Length - offset;
				default:
					return Position;
			}
		}

		public override void SetLength(long value) => BaseStream.SetLength(value + (value + 2047) / 2048 * 304);

		public override void Write(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int sectnct = 2048 - (int)(Position % 2048);
				int tmp = Math.Min(sectnct, count);
				BaseStream.Write(buffer, offset, tmp);
				count -= tmp;
				offset += tmp;
				if (tmp == sectnct)
					BaseStream.Seek(304, SeekOrigin.Current);
			}
		}
	}
}
