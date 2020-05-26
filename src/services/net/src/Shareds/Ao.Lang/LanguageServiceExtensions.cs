using System.Globalization;
using System.Linq;
using System.Threading;

namespace Ao.Lang
{
    public static class LanguageServiceExtensions
    {
        /// <summary>
        /// 获取一个语言根节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cultureName">语言文化名字</param>
        /// <returns></returns>
        public static ILanguageRoot GetRoot(this ILanguageService service,string cultureName)
        {
            if (service is null)
            {
                throw new System.ArgumentNullException(nameof(service));
            }

            if (string.IsNullOrEmpty(cultureName))
            {
                throw new System.ArgumentException("message", nameof(cultureName));
            }

            var cul=CultureInfo.GetCultureInfo(cultureName);
            return service.GetRoot(cul);
        }
        /// <summary>
        /// 获取当前线程语言的根节点
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static ILanguageRoot GetCurrentRoot(this ILanguageService service)
        {
            if (service is null)
            {
                throw new System.ArgumentNullException(nameof(service));
            }

            var cul = Thread.CurrentThread.CurrentCulture;
            return service.GetRoot(cul);
        }
        /// <summary>
        /// 获取当前线程的语言节点的值
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetCurrentValue(this ILanguageService service,string key)
        {
            var root = service.GetCurrentRoot();
            if (root!=null)
            {
                return root[key];
            }
            return null;
        }
        /// <summary>
        /// 查看语言是否支持
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cultureInfo">目标语言</param>
        /// <returns></returns>
        public static bool CultureIsSupport(this ILanguageService service,CultureInfo cultureInfo)
        {
            if (service is null)
            {
                throw new System.ArgumentNullException(nameof(service));
            }

            if (cultureInfo is null)
            {
                throw new System.ArgumentNullException(nameof(cultureInfo));
            }

            return service.SupportCultures.Any(c => c == cultureInfo);
        }
        /// <summary>
        /// <inheritdoc cref="CultureIsSupport(ILanguageService, CultureInfo)"/>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cultureName">语言文化名</param>
        /// <returns></returns>
        public static bool CultureIsSupport(this ILanguageService service, string cultureName)
        {
            var cul = CultureInfo.GetCultureInfo(cultureName);
            return service.SupportCultures.Any(c => c == cul);
        }

    }
}
