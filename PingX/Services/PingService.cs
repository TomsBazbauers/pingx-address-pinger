using PingX.Interfaces;
using PingX.Models;
using System.Net.NetworkInformation;

namespace PingX.Services
{
    public class PingService : IPingService
    {
        private readonly IPing _ping;

        public PingService(IPing ping)
        {
            _ping = ping;
        }

        public async Task<IPingResult> PingAsync(string ipAddress, int sequence)
        {
            PingReply reply = null;
            string errorMessage = null;

            try
            {
                reply = await _ping.SendPingAsync(ipAddress, 1000);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (reply != null)
            {
                return new PingResult(
                    ipAddress: ipAddress,
                    status: reply.Status,
                    roundtripTime: reply.RoundtripTime,
                    errorMessage: null,
                    sequence: sequence,
                    bufferSize: reply.Buffer.Length,
                    timeToLive: reply.Options?.Ttl
                );
            }
            else
            {
                return new PingResult(
                    ipAddress: ipAddress,
                    status: IPStatus.Unknown,
                    roundtripTime: null,
                    errorMessage: errorMessage,
                    sequence: sequence,
                    bufferSize: null,
                    timeToLive: null
                );
            }
        }
    }
}