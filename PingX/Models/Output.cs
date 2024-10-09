using PingX.Interfaces;

namespace PingX.Models
{
    public class Output : IOutput
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }

        public void ForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
