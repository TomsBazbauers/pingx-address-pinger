using System.Net.Sockets;
using System.Net;
using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Helpers
{
    public class NetworkHelper : INetworkHelper
    {
        public IList<string> GetLocalIPAddresses()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.OperationalStatus == OperationalStatus.Up)
                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .Where(a => !a.Address.ToString().StartsWith("127."))
                .Select(a => a.Address.ToString())
                .ToList();
        }
    }
}