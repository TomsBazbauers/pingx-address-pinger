using PingX.Interfaces;
using System.Net;

namespace PingX.Helpers
{
    public class InputValidator : IInputValidator
    {
        private readonly IOutputService _output;

        public InputValidator(IOutputService output)
        {
            _output = output;
        }

        public IList<string> ValidateIPAddresses(string[] args)
        {
            var ipAddresses = args?
                .Where(ip => IPAddress.TryParse(ip, out _))
                .Distinct()
                .ToList();

            if (ipAddresses == null || !ipAddresses.Any())
            {
                _output.PrintInvalidIpWarning();
                return null;
            }

            return ipAddresses;
        }
    }
}