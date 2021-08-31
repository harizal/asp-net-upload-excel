namespace BTPNS.Contracts
{
    public interface ICoreLocalizer<out T>
    {
        string this[string name] { get; }
    }
}