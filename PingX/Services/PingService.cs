using PingX.Interfaces;

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
            // try send ping
            // return pingresult
        }
    }
}