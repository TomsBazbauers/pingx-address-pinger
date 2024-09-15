namespace PingX.Interfaces
{
    public interface IOutputService
    {
        void PrintSummary(string destAddress, IList<IPingResult> results);

        void PrintIpAddresses(IList<string> sourceAddresses, IList<string> destinationAddresses);

        Task PrintSpinner(Func<Task> action);

        void PrintHelp();

        void PrintInvalidIpWarning();
    }
}
