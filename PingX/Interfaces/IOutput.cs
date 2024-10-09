namespace PingX.Interfaces
{
    public interface IOutput
    {
        void WriteLine(string message);

        void WriteLine();

        void Write(string message);

        void ResetColor();

        void ForegroundColor(ConsoleColor color);
    }
}