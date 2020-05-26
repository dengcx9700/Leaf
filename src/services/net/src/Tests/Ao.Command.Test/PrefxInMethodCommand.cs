using Ao.Command.Attributes;

namespace Ao.Command.Test
{
    public class PrefxInMethodCommand
    {
        [Prefx("inmethod")]
        public int AddOne(int a)
        {
            return a + 1;
        }
    }
}
