using PingX.Interfaces;

namespace PingX.Models
{
    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
