namespace PingX.Interfaces
{
    public interface IOutput
    {
        void WriteLine(string message);

        void ResetColor();

        void ForegroundColor(ConsoleColor color);
    }
}
