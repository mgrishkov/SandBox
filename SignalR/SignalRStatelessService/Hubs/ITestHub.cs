using System.Threading.Tasks;

namespace SignalRStatelessService.Hubs
{
    public interface ITestHub
    {
        Task SendToAll(string message);
    }
}