using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Services
{
    public class OutputService : IOutputService
    {
        private readonly IOutput _output;

        public OutputService(IOutput output)
        {
            _output = output;
        }

        public void PrintSummary(string destAddress, IList<IPingResult> results)
        {
            _output.WriteLine($"\nPing statistics for {destAddress}:");

            var sent = results.Count;
            var received = results.Count(r => r.Status == IPStatus.Success);
            var lost = sent - received;
            var lossPercent = (double)lost / sent * 100;

            Console.ForegroundColor = lossPercent switch
            {
                0 => ConsoleColor.Green,
                25 => ConsoleColor.DarkYellow,
                > 25 => ConsoleColor.Red,
                _ => Console.ForegroundColor
            };

            _output.WriteLine($"    Packets: Sent = {sent}, Received = {received}, Lost = {lost} ({lossPercent}% loss)");
            Console.ResetColor();

            if (received > 0)
            {
                var roundtripTimes = results
                    .Where(r => r.Status == IPStatus.Success)
                    .Select(r => r.RoundtripTime.Value);

                _output.WriteLine("Approximate round trip times(ms):");
                _output.WriteLine($"    Min = {(int)roundtripTimes.Min()}ms, Max = {(int)roundtripTimes.Max()}ms, Ave = {(int)roundtripTimes.Average()}ms");
            }
        }

        public void PrintOperations(IList<string> sourceAddresses, IList<string> destinationAddresses)
        {
            Console.WriteLine($"\nAvailable source addreses:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{string.Join(", ", sourceAddresses)}");
            Console.ResetColor();
            Console.WriteLine($"\nDestination addresses:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{string.Join(", ", destinationAddresses)}\n");
            Console.ResetColor();
        }

        public void PrintHelp()
        {
            Console.WriteLine("<Some help info>");
        }

        public void PrintInvalidIpWarning()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Please provide valid IP addresses to ping!");
            Console.ResetColor();
        }
    }
}