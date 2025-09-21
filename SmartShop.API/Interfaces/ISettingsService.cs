using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Interfaces
{
    public interface ISettingsService
    {
        Task<ApplicationResponse<List<Setting>>> GetAllSettingsAsync();
        Task<ApplicationResponse<Setting>> GetSettingByIdAsync(Guid id);
        Task<ApplicationResponse<Setting>> UpdateSettingAsync(Guid id, Setting setting);
        Task<ApplicationResponse<Setting>> CreateSettingAsync(Setting setting);
        Task<ApplicationResponse<Setting>> DeleteSettingAsync(Guid id);
    }
}
