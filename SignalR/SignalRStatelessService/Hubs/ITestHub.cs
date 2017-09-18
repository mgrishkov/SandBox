using System.Threading.Tasks;

namespace SignalRStatelessService.Hubs
{
    public interface ITestHub
    {
        void CallEcho(string message);
    }
}