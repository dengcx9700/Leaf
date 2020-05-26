using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ao.Core.Bytes
{
    public class BytesStream : Stream, IDisposable
    {
        public readonly int LongSize = Marshal.SizeOf(typeof(long));
        public readonly int IntSize = Marshal.SizeOf(typeof(int));
        public readonly int ChartSize = Marshal.SizeOf(typeof(char));
        public readonly int BoolSize = Marshal.SizeOf(typeof(bool));
        public readonly int ShortSize = Marshal.SizeOf(typeof(short));

        private Stream baseStream;

        public Stream BaseStream => baseStream;

        public override bool CanRead => baseStream.CanRead;

        public override bool CanSeek => baseStream.CanSeek;

        public override bool CanWrite => baseStream.CanWrite;

        public override long Length => baseStream.Length;

        public override long Position 
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public override int ReadByte()
        {
            return baseStream.ReadByte();
        }

        public int ReadInt(int offset = 0)
        {
            var bytes = new byte[IntSize];
            baseStream.Read(bytes, offset, IntSize);
            return BitConverter.ToInt32(bytes, 0);
        }
        public uint ReadUInt(int offset = 0)
        {
            var bytes = new byte[IntSize];
            baseStream.Read(bytes, offset, IntSize);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public short ReadShort(int offset = 0)
        {
            var bytes = new byte[ShortSize];
            baseStream.Read(bytes, offset, ShortSize);
            return BitConverter.ToInt16(bytes, 0);
        }

        public ushort ReadUShort(int offset = 0)
        {
            var bytes = new byte[2];
            baseStream.Read(bytes, offset, 2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public long ReadLong(int offset = 0)
        {
            var bytes = new byte[LongSize];
            baseStream.Read(bytes, offset, LongSize);
            return BitConverter.ToInt64(bytes, 0);
        }

        public ulong ReadULong(int offset = 0)
        {
            var bytes = new byte[LongSize];
            baseStream.Read(bytes, offset, LongSize);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public char ReadChart(int offset = 0)
        {
            var bytes = new byte[ChartSize];
            baseStream.Read(bytes, offset, ChartSize);
            return BitConverter.ToChar(bytes, 0);
        }
        public bool ReadBool(int offset = 0)
        {
            var bytes = new byte[BoolSize];
            baseStream.Read(bytes, offset, BoolSize);
            return BitConverter.ToBoolean(bytes, 0);
        }

        public string ReadString(int size,Encoding encoding,int offset = 0)
        {
            var bytes = new byte[size];
            baseStream.Read(bytes, offset, size);
            return encoding.GetString(bytes);
        }
        public string ReadString(int size,int offset = 0)
        {
            return ReadString(size, Encoding.Default, offset);
        }
        public new void Dispose()
        {
            baseStream?.Dispose();
            baseStream = null;
            base.Dispose();
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }
    }
}
