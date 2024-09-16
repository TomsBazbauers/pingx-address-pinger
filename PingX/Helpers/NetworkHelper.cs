using System.Net.Sockets;
using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Helpers
{
    public class NetworkHelper : INetworkHelper
    {
        private readonly INetworkInterfaceProvider _networkInterfaceProvider;

        public NetworkHelper(INetworkInterfaceProvider networkInterfaceProvider)
        {
            _networkInterfaceProvider = networkInterfaceProvider;
        }

        public IList<string> GetLocalIPAddresses()
        {
            return _networkInterfaceProvider.GetAllNetworkInterfaces()
                .Where(i => i.OperationalStatus == OperationalStatus.Up)
                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .Where(a => !a.Address.ToString().StartsWith("127."))
                .Select(a => a.Address.ToString())
                .ToList();
        }
    }
}