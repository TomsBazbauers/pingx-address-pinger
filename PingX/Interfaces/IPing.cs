using System.Net.NetworkInformation;

namespace PingX.Interfaces
{
    public interface IPing
    {
        Task<PingReply> SendPingAsync(string ipAddress, int timeout);
    }
}
