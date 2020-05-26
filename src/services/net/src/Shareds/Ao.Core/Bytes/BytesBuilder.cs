using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ao.Core.Bytes
{
    public class BytesBuilder : List<byte>
    {
        public BytesBuilder Add(int value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }

        public BytesBuilder Add(short value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(ushort value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(long value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(ulong value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(char value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(string value,Encoding encoding)
        {
            var bytes = encoding.GetBytes(value);
            AddRange(bytes);
            return this;
        }
        public BytesBuilder Add(string value)
        {
            Add(value, Encoding.Default);
            return this;
        }
        public BytesBuilder Add(float value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(double value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }
        public BytesBuilder Add(bool value)
        {
            AddRange(BitConverter.GetBytes(value).Reverse());
            return this;
        }

    }
}
