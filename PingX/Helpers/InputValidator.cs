using PingX.Interfaces;
using System.Net;

namespace PingX.Helpers
{
    public class InputValidator : IInputValidator
    {
        public IList<string> ValidateIPAddresses(string[] args)
        {
            var ipAddresses = args?
                .Where(ip => IPAddress.TryParse(ip, out _))
                .Where(ip => !ip.StartsWith("127.0"))
                .Distinct()
                .ToList();

            return ipAddresses == null || !ipAddresses.Any() ? null : ipAddresses;
        }
    }
}