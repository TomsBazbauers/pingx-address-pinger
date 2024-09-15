using PingX.Interfaces;

namespace PingX.Services
{
    public class OutputService : IOutputService
    {
        private readonly IOutput _output;

        public OutputService(IOutput output)
        {
            _output = output;
        }

        public void PrintSummary(String destAddress, IList<IPingResult> results)
        {
            // ending stats console output/ui logic
        }

        public void PrintOperations(string source, IList<string> destinations)
        {
            // print pinging...
        }

        public void PrintHelp()
        {
            //print -h
            // use json/yaml??
        }
    }
}
