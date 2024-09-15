using PingX.Interfaces;
using System.Net.NetworkInformation;

namespace PingX.Models
{
    public class PingResult : IPingResult
    {
        public string IPAddress { get; }

        public IPStatus Status { get; }

        public long? RoundtripTime { get; }

        public string ErrorMessage { get; }

        public int Sequence { get; }

        public int? BufferSize { get; }

        public int? TimeToLive { get; }

        public PingResult(string ipAddress, IPStatus status,
            long? roundtripTime, string errorMessage, int sequence, int? bufferSize, int? timeToLive)
        {
            IPAddress = ipAddress;
            Status = status;
            RoundtripTime = roundtripTime;
            ErrorMessage = errorMessage;
            Sequence = sequence;
            BufferSize = bufferSize;
            TimeToLive = timeToLive;
        }
    }
}