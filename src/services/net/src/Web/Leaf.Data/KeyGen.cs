using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Data
{
    public static class KeyGen
    {
        public const char Split = '-';

        public static string Gen(params object[] blocks)
        {
            return string.Join(Split, blocks);
        }
    }
}
