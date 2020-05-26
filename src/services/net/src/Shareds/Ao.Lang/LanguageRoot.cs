using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Ao.Lang
{
    /// <summary>
    /// 语言根节点
    /// </summary>
    public class LanguageRoot : ConfigurationRoot, ILanguageRoot
    {
        public LanguageRoot(CultureInfo culture,IList<IConfigurationProvider> providers)
            : base(providers)
        {
            Culture = culture;
        }
        /// <summary>
        /// 语言
        /// </summary>
        public CultureInfo Culture { get; }
    }
}
