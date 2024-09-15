using System.Net.NetworkInformation;

namespace PingX.Interfaces
{
    public interface IPingResult
    {
        string IPAddress { get; }
        IPStatus Status { get; }
        long? RoundtripTime { get; }
        string ErrorMessage { get; }
        int Sequence { get; }
        int? BufferSize { get; }
        int? TimeToLive { get; }
    }
}