using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Services.Wrappers
{
    public class PingWrapper : IPing
    {
        private readonly Ping _ping;

        public PingWrapper()
        {
            _ping = new Ping();
        }

        public Task<PingReply> SendPingAsync(string ipAddress, int timeout)
        {

            return _ping.SendPingAsync(ipAddress, timeout);
        }
    }
}