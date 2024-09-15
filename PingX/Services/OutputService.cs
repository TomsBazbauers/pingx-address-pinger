using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Services
{
    public class OutputService : IOutputService
    {
        private readonly IOutput _output;
        static readonly object _consoleLock = new object();

        public OutputService(IOutput output)
        {
            _output = output;
        }

        public void PrintSummary(string destAddress, IList<IPingResult> results)
        {
            string statsHeaderMessage = $"\nPing statistics for {destAddress}:";
            PrintMessage(statsHeaderMessage, ConsoleColor.White);

            var sent = results.Count;
            var received = results.Count(r => r.Status == IPStatus.Success);
            var lost = sent - received;
            var lossPercent = (double)lost / sent * 100;
            string packetsMessage = $"    Packets: Sent = {sent}, Received = {received}, Lost = {lost} ({lossPercent}% loss)";

            ConsoleColor color = lossPercent switch
            {
                0 => ConsoleColor.Green,
                25 => ConsoleColor.DarkYellow,
                > 25 => ConsoleColor.Red,
                _ => ConsoleColor.White
            };

            PrintMessage(packetsMessage, color);

            if (received > 0)
            {
                var roundtripTimes = results
                    .Where(r => r.Status == IPStatus.Success)
                    .Select(r => r.RoundtripTime.Value);

                string roundtripMessage = "Approximate round trip times(ms):";
                string timeStatsMessage = $"    Min = {(int)roundtripTimes.Min()}ms, " +
                    $"Max = {(int)roundtripTimes.Max()}ms, Ave = {(int)roundtripTimes.Average()}ms";

                PrintMessage(roundtripMessage, ConsoleColor.White);
                PrintMessage(timeStatsMessage, ConsoleColor.White);
            }
        }

        public void PrintIpAddresses(IList<string> sourceAddresses, IList<string> destinationAddresses)
        {
            PrintMessage("\nAvailable source addresses:", ConsoleColor.White);
            PrintMessage(string.Join(", ", sourceAddresses), ConsoleColor.Blue);

            PrintMessage("\nDestination addresses:", ConsoleColor.White);
            PrintMessage(string.Join(", ", destinationAddresses), ConsoleColor.Blue);
        }

        public void PrintHelp()
        {
            PrintMessage("<Some help info>", ConsoleColor.White);
        }

        public void PrintInvalidIpWarning()
        {
            PrintMessage("Please provide valid IP addresses to ping!", ConsoleColor.Red);
        }

        private void PrintMessage(string message, ConsoleColor color)
        {
            _output.ForegroundColor(color);
            _output.WriteLine(message);
            _output.ResetColor();
        }

        public async Task PrintSpinner(Func<Task> action)
        {
            var spinnerElements = new[] { '/', '\\', '|', '-' };
            int spinnerIndex = 0;
            bool running = true;

            var spinnerTask = Task.Run(async () =>
            {
                while (running)
                {
                    lock (_consoleLock)
                    {
                        Console.Write($"\b{spinnerElements[spinnerIndex]}");
                        spinnerIndex = (spinnerIndex + 1) % spinnerElements.Length;
                    }

                    await Task.Delay(100);
                }
            });

            await action();
            running = false;
            await spinnerTask;

            Console.Write("\b \b");
            Console.WriteLine();
        }
    }
}