using PingX.Helpers;
using PingX.Interfaces;
using PingX.Models;
using PingX.Services;
using PingX.Services.Wrappers;
using System.Collections.Concurrent;

namespace PingX
{
    class Program
    {
        private readonly IPingService _pingService;
        private readonly IOutputService _outputService;
        private readonly IInputValidator _inputValidator;
        private readonly INetworkHelper _networkHelper;

        public Program(IPingService pingService,
            IInputValidator inputValidator, IOutputService outputService, INetworkHelper networkHelper)
        {
            _inputValidator = inputValidator;
            _pingService = pingService;
            _outputService = outputService;
            _networkHelper = networkHelper;
        }

        static async Task Main(string[] args)
        {
            IPing pingWrapper = new PingWrapper();
            IPingService pingService = new PingService(pingWrapper);
            IOutput output = new Output();
            IOutputService outputService = new OutputService(output);
            IInputValidator inputValidator = new InputValidator(outputService);
            INetworkHelper networkHelper = new NetworkHelper();

            var program = new Program(pingService, inputValidator, outputService, networkHelper);
            await program.Run(args);
        }

        public async Task Run(string[] args)
        {
            var ipAddresses = _inputValidator.ValidateIPAddresses(args);

            if (ipAddresses == null)
            {
                return;
            }

            var sourceIPs = _networkHelper.GetLocalIPAddresses();
            var resultsPerIp = new ConcurrentDictionary<string, IList<IPingResult>>();
            _outputService.PrintIpAddresses(sourceIPs, ipAddresses);

            await _outputService.PrintSpinner(async () =>
            {
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
            });

            ipAddresses.ToList().ForEach(ip =>
            {
                if (resultsPerIp.TryGetValue(ip, out var results))
                {
                    _outputService.PrintSummary(ip, results);
                }
            });
        }

    }
}