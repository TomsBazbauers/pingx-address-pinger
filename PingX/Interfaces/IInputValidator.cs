namespace PingX.Interfaces
{
    public interface IInputValidator
    {
        IList<string> ValidateIPAddresses(string[] args);
    }
}
