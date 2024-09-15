namespace PingX.Interfaces
{
    public interface IPingService
    {
        Task<IPingResult> PingAsync(string ipAddress, int sequence);
    }
}
