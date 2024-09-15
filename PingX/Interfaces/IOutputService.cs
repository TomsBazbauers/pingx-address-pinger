namespace PingX.Interfaces
{
    public interface IOutputService
    {
        void PrintSummary(string destAddress, IList<IPingResult> results);

        void PrintOperations(IList<string> sourceAddresses, IList<string> destinationAddresses);

        void PrintHelp();

        void PrintInvalidIpWarning();
    }
}
