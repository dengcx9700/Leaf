using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Ao.Lang
{
    public class CurrentCultureLanguageMetadata : LanguageMetadata
    {
        private readonly IConfigurationSource[] sources;

        public CurrentCultureLanguageMetadata(params IConfigurationSource[] sources)
        {
            this.sources = sources;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override CultureInfo Culture =>Thread.CurrentThread.CurrentCulture;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override IConfigurationSource[] GetConfigurationSources()
        {
            return sources;
        }
    }
}
