using SmartShop.API.Interfaces;

namespace SmartShop.API.Common
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
