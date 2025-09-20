using Microsoft.EntityFrameworkCore;
using SmartShop.API.Models;

namespace SmartShop.API.Services
{
    public class SequenceService
    {
        private readonly DbContext _context;

        public SequenceService(DbContext context)
        {
            _context = context;
        }

        public async Task<string> GetNextSequenceAsync(string key, bool increment)
        {
            // Try to find the existing sequence config by key
            var config = await _context.Set<SequenceConfig>()
                .FirstOrDefaultAsync(c => c.Key == key);

            int nextNumber;

            if (config == null)
            {
                // Initialize if not found
                nextNumber = 1;
                config = new SequenceConfig
                {
                    Id = Guid.NewGuid(),
                    Key = key,
                    Description = $"Sequence for {key}",
                    Prefix = string.Empty,
                    Length = 6,
                    Value = nextNumber
                };
                _context.Add(config);
            }
            else
            {
                if (increment)
                {
                    // increment
                    nextNumber = config.Value + 1;
                    config.Value = nextNumber;
                    _context.Update(config);
                }
                else
                {
                    // do not increment, just return current value
                    nextNumber = config.Value;
                }
            }

            await _context.SaveChangesAsync();

            // Format the sequence
            var padded = nextNumber.ToString().PadLeft(config.Length, '0');
            return string.IsNullOrEmpty(config.Prefix) ? padded : $"{config.Prefix}-{padded}";
        }
    }

}
