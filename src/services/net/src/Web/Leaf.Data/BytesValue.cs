using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Leaf.Model.ResponseEntity
{
    public struct BytesValue<TValue>
    {
        public BytesValue(byte[] bytes)
        {
            this.bytes = new Lazy<byte[]>(() => bytes);
            value = new Lazy<TValue>(() => JsonSerializer.Deserialize<TValue>(bytes));
        }
        public BytesValue(TValue value)
        {
            this.bytes = new Lazy<byte[]>(() => JsonSerializer.SerializeToUtf8Bytes(value));
            this.value = new Lazy<TValue>(() => value);
        }
        private readonly Lazy<byte[]> bytes;
        private readonly Lazy<TValue> value;

        public byte[] Bytes => bytes.Value;

        public TValue Value => value.Value;
    }
}
