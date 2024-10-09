using FluentAssertions;
using PingX.Helpers;

namespace PingX.Tests.Helpers
{
    public class NetworkHelperTests
    {
        [Fact]
        public void GetLocalIPAddresses_ShouldReturnNonEmptyList_WhenNetworkIsAvailable()
        {
            // Arrange
            var networkHelper = new NetworkHelper();

            // Act
            var result = networkHelper.GetLocalIPAddresses();

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetLocalIPAddresses_ShouldNotReturnLoopbackAddresses()
        {
            // Arrange
            var networkHelper = new NetworkHelper();

            // Act
            var result = networkHelper.GetLocalIPAddresses();

            // Assert
            result.Should().OnlyContain(ip => !ip.StartsWith("127."));
        }
    }
}