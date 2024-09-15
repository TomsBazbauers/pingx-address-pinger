using System.Net.Sockets;
using System.Net;
using PingX.Interfaces;

namespace PingX.Helpers
{
    public class NetworkHelper :INetworkHelper
    {
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "<N/A>";
        }
    }
}