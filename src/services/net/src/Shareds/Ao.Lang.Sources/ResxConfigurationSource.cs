using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Lang.Sources
{
    /// <summary>
    /// 表示resx配置源
    /// </summary>
    public class ResxConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ResxConfigurationProvider(this);
        }
    }
}
