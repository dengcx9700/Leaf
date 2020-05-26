using Ao.Command.Attributes;

namespace Ao.Command.Test
{
    [Prefx("calc")]
    public class PrefxCommand
    {
        public int AddOne(int a)
        {
            return a + 1;
        }
    }
}
