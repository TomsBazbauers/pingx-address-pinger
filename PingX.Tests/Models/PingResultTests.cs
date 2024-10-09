using FluentAssertions;
using PingX.Models;
using System.Net.NetworkInformation;


namespace PingX.Tests.Models
{
    public class PingResultTests
    {
        [Theory]
        [InlineData("1.1.1.1", IPStatus.Success, 10L, null, 1, 32, 64)]
        [InlineData("8.8.8.8", IPStatus.TimedOut, null, "Timeout", 2, null, null)]
        public void Constructor_ShouldSetCorrectValues(
        string ipAddress, IPStatus status, long? roundtripTime, string errorMessage, int sequence, int? bufferSize, int? ttl)
        {
            // Act
            var result = new PingResult(ipAddress, status, roundtripTime, errorMessage, sequence, bufferSize, ttl);

            // Assert
            result.IPAddress.Should().Be(ipAddress);
            result.Status.Should().Be(status);
            result.RoundtripTime.Should().Be(roundtripTime);
            result.ErrorMessage.Should().Be(errorMessage);
            result.Sequence.Should().Be(sequence);
            result.BufferSize.Should().Be(bufferSize);
            result.TTL.Should().Be(ttl);
        }

        [Theory]
        [InlineData("8.8.8.8", IPStatus.Success, 50L, null, 1, 32, 64)]
        [InlineData("192.168.1.1", IPStatus.Success, 100L, null, 2, 64, 128)]
        [InlineData("10.0.0.1", IPStatus.TimedOut, null, "Request timed out", 3, null, null)]
        [InlineData("255.255.255.255", IPStatus.Success, 10L, null, 4, 32, 64)]
        public void Constructor_ShouldInitializePropertiesCorrectly(
        string ipAddress, IPStatus status, long? roundtripTime, string errorMessage, int sequence, int? bufferSize, int? ttl)
        {
            // Act
            var result = new PingResult(ipAddress, status, roundtripTime, errorMessage, sequence, bufferSize, ttl);

            // Assert
            result.IPAddress.Should().Be(ipAddress);
            result.Status.Should().Be(status);
            result.RoundtripTime.Should().Be(roundtripTime);
            result.ErrorMessage.Should().Be(errorMessage);
            result.Sequence.Should().Be(sequence);
            result.BufferSize.Should().Be(bufferSize);
            result.TTL.Should().Be(ttl);
        }

        [Fact]
        public void Constructor_ShouldHandleNullValues()
        {
            // Arrange
            var ipAddress = "8.8.8.8";
            var status = IPStatus.TimedOut;
            long? roundtripTime = null;
            string errorMessage = "Request timed out";
            int sequence = 2;
            int? bufferSize = null;
            int? ttl = null;

            // Act
            var result = new PingResult(ipAddress, status, roundtripTime, errorMessage, sequence, bufferSize, ttl);

            // Assert
            result.IPAddress.Should().Be(ipAddress);
            result.Status.Should().Be(status);
            result.RoundtripTime.Should().BeNull();
            result.ErrorMessage.Should().Be(errorMessage);
            result.Sequence.Should().Be(sequence);
            result.BufferSize.Should().BeNull();
            result.TTL.Should().BeNull();
        }
    }
}