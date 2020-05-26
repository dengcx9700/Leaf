using Ao.Command.Attributes;

namespace Ao.Command.Test
{
    public class TestNoCommand
    {
        [NotCommand]
        public int Run()
        {
            return 1;
        }
    }
}
