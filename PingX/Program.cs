using PingX.Helpers;
using PingX.Interfaces;
using PingX.Models;
using PingX.Services;
using PingX.Services.Wrappers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace PingX
{
    class Program
    {
        private readonly IInputValidator _inputValidator;
        private readonly IPingService _pingService;
        private readonly IOutputService _outputService;

        public Program(IInputValidator inputValidator,
            IPingService pingService, IOutputService outputService)
        {
            _inputValidator = inputValidator;
            _pingService = pingService;
            _outputService = outputService;
        }

        static async Task Main(string[] args)
        {
            IPing pingWrapper = new PingWrapper();
            IPingService pingService = new PingService(pingWrapper);
            IOutput output = new Output();
            IOutputService outputService = new OutputService(output);
            IInputValidator inputValidator = new InputValidator(outputService);

            var program = new Program(inputValidator, pingService, outputService);
            await program.Run(args);
        }

        public async Task Run(string[] args)
        {
            var ipAddresses = _inputValidator.ValidateIPAddresses(args);

            if (ipAddresses == null)
            {
                return;
            }

            var sourceIP = GetLocalIPAddress();
            _outputService.PrintOperations(sourceIP, ipAddresses);

            var resultsPerIp = new ConcurrentDictionary<string, IList<IPingResult>>();

            var pingTasks = ipAddresses.Select(async ip =>
            {
                IList<IPingResult> results = new List<IPingResult>();

                for (int i = 0; i < 4; i++)
                {
                    var result = await _pingService.PingAsync(ip, i + 1);
                    results.Add(result);
                    await Task.Delay(750);
                }

                resultsPerIp[ip] = results;
            }).ToList();

            await Task.WhenAll(pingTasks);

            foreach (var ip in ipAddresses)
            {
                if (resultsPerIp.TryGetValue(ip, out var results))
                {
                    _outputService.PrintSummary(ip, results);
                }
            }
        }

        public static string GetLocalIPAddress()
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
