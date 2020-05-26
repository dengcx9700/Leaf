using Microsoft.Extensions.Configuration;
using System.Collections;
using System.IO;
using System.Resources;

namespace Ao.Lang.Sources
{
    /// <summary>
    /// 表示resx文件的配置提供者
    /// </summary>
    public class ResxConfigurationProvider : FileConfigurationProvider
    {
        public ResxConfigurationProvider(FileConfigurationSource source) 
            : base(source)
        {
        }
        public override void Load(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using(var mem=new StreamReader(stream))
            {
                var text = mem.ReadToEnd();
                using (var reader = ResXResourceReader.FromFileContents(text))
                {
                    var en = reader.GetEnumerator();
                    while (en.MoveNext())
                    {
                        var entity = (DictionaryEntry)en.Current;
                        if (entity.Key != null && !Data.ContainsKey(entity.Key.ToString()))
                        {
                            Data.Add(entity.Key.ToString(), entity.Value.ToString());
                        }
                    }
                }
            }
        }
    }
}
