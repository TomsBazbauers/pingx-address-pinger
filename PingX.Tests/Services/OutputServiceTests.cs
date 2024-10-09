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
        [InlineData(4, 0, 100, ConsoleColor.Red)]
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
            var expectedPacketMessage = $"Packets: Sent = {sent}, Received = {received}, Lost = {sent - received} ({expectedLossPercent}% loss)";
            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains("Ping statistics for 8.8.8.8:"));
            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains(expectedPacketMessage));

            if (received > 0)
            {
                var minTime = 20;
                var maxTime = 20 + (received - 1) * 5;
                var avgTime = (minTime + maxTime) / 2;

                var expectedRttMessage = $"Min = {minTime}ms, Max = {maxTime}ms, Ave = {avgTime}ms";

                mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains("Approximate round trip times(ms):"));
                mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.WriteLine) && i.Arguments[0].ToString().Contains(expectedRttMessage));
            }

            mockOutput.Invocations.Should().ContainSingle(i => i.Method.Name == nameof(IOutput.ForegroundColor) && (ConsoleColor)i.Arguments[0] == expectedColor);
            mockOutput.Invocations.Should().Contain(i => i.Method.Name == nameof(IOutput.ResetColor));
        }

        [Theory]
        [InlineData(new[] { "192.168.1.1" }, new[] { "8.8.8.8" }, "192.168.1.1", "8.8.8.8")]
        [InlineData(new[] { "192.168.1.1", "10.0.0.1", "172.16.0.1" }, new[] { "8.8.8.8", "1.1.1.1", "4.4.4.4" }, "192.168.1.1, 10.0.0.1, 172.16.0.1", "8.8.8.8, 1.1.1.1, 4.4.4.4")]
        public void PrintIpAddresses_ShouldPrintSourceAndDestinationAddresses(string[] sourceAddresses, string[] destinationAddresses, string expectedSourceAddresses, string expectedDestinationAddresses)
        {
            // Arrange
            var mockOutput = new Mock<IOutput>();
            var outputService = new OutputService(mockOutput.Object);

            // Act
            outputService.PrintIpAddresses(sourceAddresses.ToList(), destinationAddresses.ToList());

            // Assert
            var expectedInvocations = new List<(string methodName, object argument)>
            {
                (nameof(IOutput.ForegroundColor), ConsoleColor.White),
                (nameof(IOutput.WriteLine), "Available source addresses:"),
                (nameof(IOutput.ResetColor), null),

                (nameof(IOutput.ForegroundColor), ConsoleColor.Blue),
                (nameof(IOutput.WriteLine), expectedSourceAddresses),
                (nameof(IOutput.ResetColor), null),

                (nameof(IOutput.ForegroundColor), ConsoleColor.White),
                (nameof(IOutput.WriteLine), "Destination addresses:"),
                (nameof(IOutput.ResetColor), null),

                (nameof(IOutput.ForegroundColor), ConsoleColor.Blue),
                (nameof(IOutput.WriteLine), expectedDestinationAddresses),
                (nameof(IOutput.ResetColor), null),
            };

            for (int i = 0; i < expectedInvocations.Count; i++)
            {
                var (expectedMethod, expectedArgument) = expectedInvocations[i];
                var actualInvocation = mockOutput.Invocations[i];

                Assert.Equal(expectedMethod, actualInvocation.Method.Name);

                if (expectedArgument != null)
                {
                    var argument = actualInvocation.Arguments[0];
                    if (expectedArgument is ConsoleColor)
                    {
                        Assert.Equal((ConsoleColor)expectedArgument, argument);
                    }
                    else
                    {
                        Assert.Contains(expectedArgument.ToString(), argument.ToString());
                    }
                }
            }
        }

        [Fact]
        public void PrintInvalidIpWarning_ShouldPrintWarningMessageInRed()
        {
            // Arrange
            var mockOutput = new Mock<IOutput>();
            var outputService = new OutputService(mockOutput.Object);

            // Act
            outputService.PrintInvalidIpWarning();

            // Assert
            mockOutput.Verify(o => o.ForegroundColor(ConsoleColor.Red), Times.Once);
            mockOutput.Verify(o => o.WriteLine("Please provide valid IP addresses to ping!"), Times.Once);
            mockOutput.Verify(o => o.ResetColor(), Times.Once);
        }

        [Theory]
        [InlineData("Hello, World!", ConsoleColor.Green)]
        [InlineData("Warning: Invalid input!", ConsoleColor.Yellow)]
        [InlineData("Error: Connection failed!", ConsoleColor.Red)]
        public void PrintMessage_ShouldSetForegroundColor_PrintMessage_AndResetColor(string message, ConsoleColor color)
        {
            // Arrange
            var mockOutput = new Mock<IOutput>();
            var outputService = new OutputService(mockOutput.Object);

            // Act
            outputService.GetType()
                .GetMethod("PrintMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(outputService, new object[] { message, color });

            // Assert
            mockOutput.Verify(o => o.ForegroundColor(color), Times.Once);
            mockOutput.Verify(o => o.WriteLine(message), Times.Once);
            mockOutput.Verify(o => o.ResetColor(), Times.Once);
        }
    }
}