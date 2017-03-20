using System.Threading.Tasks;

namespace SampleServer.Hubs
{
    public interface ITestHub
    {
        void CallEcho(string message);
    }
}