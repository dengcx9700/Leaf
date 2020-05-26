using Ao.Command.Attributes;

namespace Ao.Command.Test
{
    public class AliasCommand
    {
        [Alias("ao")]
        public int AddOne([Alias("aa")]int a)
        {
            return a + 1;
        }

    }
}
