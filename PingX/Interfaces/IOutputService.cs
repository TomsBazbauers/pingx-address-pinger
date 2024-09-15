namespace PingX.Interfaces
{
    public interface IOutputService
    {
        void PrintSummary(string destAddress, IList<IPingResult> results);

        void PrintOperations(string source, IList<string> destinations);

        void PrintHelp();

        void PrintInvalidIpWarning();
    }
}
