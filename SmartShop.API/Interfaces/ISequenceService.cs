namespace SmartShop.API.Interfaces
{
    public interface ISequenceService
    {
        Task<string> GetNextSequenceAsync(string key, bool IsIncrement);
    }

}
