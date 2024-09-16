using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using PingX.Helpers;
using PingX.Interfaces;
using PingX.Models;
using PingX.Services.Wrappers;
using PingX.Services;

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
        var serviceProvider = new ServiceCollection()
            .AddTransient<IPing, PingWrapper>()
            .AddTransient<IPingService, PingService>()
            .AddTransient<IInputValidator, InputValidator>()
            .AddSingleton<IOutput, Output>()
            .AddSingleton<IOutputService, OutputService>()
            .AddSingleton<INetworkInterfaceProvider, NetworkInterfaceProvider>()
            .AddSingleton<INetworkHelper, NetworkHelper>()
            .AddSingleton<Program>()
            .BuildServiceProvider();

        var program = serviceProvider.GetRequiredService<Program>();
        await program.Run(args);
    }

    public async Task Run(string[] args)
    {
        var ipAddresses = _inputValidator.ValidateIPAddresses(args);

        if (ipAddresses == null)
        {
            _outputService.PrintInvalidIpWarning();
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