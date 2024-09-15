using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingX.Interfaces
{
    public interface IPing
    {
        // ping wrapper, copy ping
        Task<PingReply> SendPingAsync(string ipAddress, int timeout);
    }
}
