using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Helpers
{
    public class NetworkInterfaceProvider : INetworkInterfaceProvider
    {
        public IEnumerable<NetworkInterface> GetAllNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }
    }
}