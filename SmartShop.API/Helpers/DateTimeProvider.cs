using SmartShop.API.Interfaces;

namespace SmartShop.API.Helpers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
