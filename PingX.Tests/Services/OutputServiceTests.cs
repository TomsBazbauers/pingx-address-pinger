using FluentAssertions;
using Moq;
using PingX.Interfaces;
using PingX.Models;
using PingX.Services;
using System.Net.NetworkInformation;

namespace PingX.Tests.Services
{
    public class OutputServiceTests
    {
        [Theory]
        [InlineData(4, 4, 0, ConsoleColor.Green)]
        [InlineData(4, 3, 25, ConsoleColor.DarkYellow)] 
        [InlineData(4, 2, 50, ConsoleColor.Red)] 
        public void PrintSummary_ShouldPrintCorrectMessages(int sent, int received, double expectedLossPercent, ConsoleColor expectedColor)
        {
            // Arrange
            var mockOutput = new Mock<IOutput>();
            var outputService = new OutputService(mockOutput.Object);
            var results = new List<IPingResult>();

            for (int i = 0; i < received; i++)
            {
                results.Add(new PingResult("8.8.8.8", IPStatus.Success, 20 + (i * 5), null, i + 1, 32, 64));
            }

            for (int i = received; i < sent; i++)
            {
                results.Add(new PingResult("8.8.8.8", IPStatus.TimedOut, null, null, i + 1, null, null));
            }

            // Act
            outputService.PrintSummary("8.8.8.8", results);

            // Assert
            var expectedPacketsMessage = $"Packets: Sent = {sent}, Received = {received}, Lost = {sent - received} ({expectedLossPercent}% loss)";
            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains("Ping statistics for 8.8.8.8:"));
            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains(expectedPacketsMessage));
            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains("Approximate round trip times(ms):"));
            if (received > 0)
            {
                mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains("Min = 20ms, Max = 25ms, Ave = 22ms"));
            }
            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.ForegroundColor) && (ConsoleColor)i.Arguments[0] == expectedColor);
            mockOutput.Invocations.Should().Contain(i => i.Method.Name == nameof(IOutput.ResetColor));
        }
    }
}