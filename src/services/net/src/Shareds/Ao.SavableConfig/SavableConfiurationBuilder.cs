using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 可保持的配置生成器
    /// </summary>
    public class SavableConfiurationBuilder : ConfigurationBuilder, ISavableConfigurationBuilder
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public new ISavableConfigurationRoot Build()
        {
            var providers = this.Sources.Select(s => s.Build(this)).ToArray();
            return new SavableConfigurationRoot(providers);
        }
    }
}
