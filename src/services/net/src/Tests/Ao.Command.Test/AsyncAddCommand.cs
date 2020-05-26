using System.Threading.Tasks;

namespace Ao.Command.Test
{
    public class AsyncAddCommand
    {
        public Task<int> AddAsync(int a,int b)
        {
            return Task.FromResult(a + b);
        }
    }
}
