using System.Net.NetworkInformation;

namespace PingX.Interfaces
{
    public interface INetworkInterfaceProvider
    {
        IEnumerable<NetworkInterface> GetAllNetworkInterfaces();
    }
}