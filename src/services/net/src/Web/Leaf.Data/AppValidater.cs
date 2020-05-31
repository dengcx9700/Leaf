using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Leaf.Data
{
    public static class AppValidater
    {
        public static readonly string FaileAppKey = "-fail-";

        public static readonly string HeaderAppKeyKey = "app_key_";

        public static readonly string HeaderSessionKey = "app_session_";
        public static readonly string HeaderSessionCreateTimeKey = "app_session_createtime_";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MakeAppKeyKey(string key)
        {
            return HeaderAppKeyKey + key;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MakeSessionCreateTimeKey(string key)
        {
            return HeaderSessionCreateTimeKey + key;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MakeSessionKey(string appKey)
        {
            return HeaderSessionKey + appKey;
        }

    }
}
