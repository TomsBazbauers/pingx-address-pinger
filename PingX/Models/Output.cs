using PingX.Interfaces;

namespace PingX.Models
{
    public class Output : IOutput
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
