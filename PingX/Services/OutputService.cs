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

            _output.WriteLine($"    Packets: Sent = {sent}, Received = {received}, Lost = {lost} ({lossPercent}% loss)");

            if (received > 0)
            {
                var roundtripTimes = results
                    .Where(r => r.Status == IPStatus.Success)
                    .Select(r => r.RoundtripTime.Value);

                _output.WriteLine("Approximate round trip times(ms):");
                _output.WriteLine($"    Min = {(int)roundtripTimes.Min()}ms, Max = {(int)roundtripTimes.Max()}ms, Ave = {(int)roundtripTimes.Average()}ms");
            }
        }

        public void PrintOperations(string source, IList<string> destinations)
        {
            Console.WriteLine($"Source: {source}, Pinging: {string.Join(", ", destinations)}...");
        }

        public void PrintHelp()
        {
            Console.WriteLine("<Some help info>");
        }

        public void PrintInvalidIpWarning()
        {
            Console.WriteLine("Please provide valid IP addresses to ping!");
        }
    }
}