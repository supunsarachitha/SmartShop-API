using Microsoft.EntityFrameworkCore;
using SmartShop.API.Helpers;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{
    //public class SettingsService
    //{
    //    private readonly SmartShopDbContext _context;
    //    private readonly IDateTimeProvider _dateTimeProvider;

    //    public SettingsService(SmartShopDbContext context, IDateTimeProvider dateTimeProvider)
    //    {
    //        _context = context;
    //        _dateTimeProvider = dateTimeProvider;
    //    }

    //    public async Task<ApplicationResponse<List<Setting>>> GetAllSettingsAsync()
    //    {
    //        try
    //        {
    //            var settings = await _context.Settings.ToListAsync();
    //            return ResponseFactory.CreateSuccessResponse(
    //                settings,
    //                "Settings retrieved successfully.",
    //                StatusCodes.Status200OK);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ResponseFactory.CreateErrorResponse<List<Setting>>(
    //                "Failed to retrieve settings.",
    //                "Exception",
    //                ex.Message,
    //                StatusCodes.Status500InternalServerError);
    //        }
    //    }

    //    public async Task<ApplicationResponse<Setting>> GetSettingByIdAsync(int id)
    //    {
    //        try
    //        {
    //            var setting = await _context.Settings.FindAsync(id);
    //            if (setting == null)
    //            {
    //                return ResponseFactory.CreateErrorResponse<Setting>(
    //                    "Setting not found.",
    //                    "Id",
    //                    "No setting found with the specified ID.",
    //                    StatusCodes.Status404NotFound);
    //            }
    //            return ResponseFactory.CreateSuccessResponse(
    //                setting,
    //                "Setting retrieved successfully.",
    //                StatusCodes.Status200OK);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ResponseFactory.CreateErrorResponse<Setting>(
    //                "Failed to retrieve setting.",
    //                "Exception",
    //                ex.Message,
    //                StatusCodes.Status500InternalServerError);
    //        }
    //    }

    //    public async Task<ApplicationResponse<Setting>> CreateSettingAsync(Setting setting)
    //    {
    //        try
    //        {
    //            setting.CreatedDate = _dateTimeProvider.UtcNow;
    //            setting.UpdatedDate = _dateTimeProvider.UtcNow;

    //            _context.Settings.Add(setting);
    //            await _context.SaveChangesAsync();

    //            return ResponseFactory.CreateSuccessResponse(
    //                setting,
    //                "Setting created successfully.",
    //                StatusCodes.Status201Created);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ResponseFactory.CreateErrorResponse<Setting>(
    //                "Setting creation failed.",
    //                "Exception",
    //                ex.Message,
    //                StatusCodes.Status500InternalServerError);
    //        }
    //    }

    //    public async Task<ApplicationResponse<Setting>> UpdateSettingAsync(int id, Setting updatedSetting)
    //    {
    //        if (id != updatedSetting.Id)
    //        {
    //            return ResponseFactory.CreateErrorResponse<Setting>(
    //                "Setting ID mismatch.",
    //                "Id",
    //                "The ID in the URL does not match the ID in the payload.",
    //                StatusCodes.Status400BadRequest);
    //        }

    //        var existingSetting = await _context.Settings.FindAsync(id);
    //        if (existingSetting == null)
    //        {
    //            return ResponseFactory.CreateErrorResponse<Setting>(
    //                "Setting not found.",
    //                "Setting",
    //                "No setting found with the specified ID.",
    //                StatusCodes.Status404NotFound);
    //        }

    //        existingSetting.Key = updatedSetting.Key;
    //        existingSetting.Value = updatedSetting.Value;
    //        existingSetting.DataType = updatedSetting.DataType;
    //        existingSetting.Description = updatedSetting.Description;
    //        existingSetting.UpdatedDate = _dateTimeProvider.UtcNow;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //            return ResponseFactory.CreateSuccessResponse(
    //                existingSetting,
    //                "Setting updated successfully.",
    //                StatusCodes.Status200OK);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ResponseFactory.CreateErrorResponse<Setting>(
    //                "Setting update failed.",
    //                "Exception",
    //                ex.Message,
    //                StatusCodes.Status500InternalServerError);
    //        }
    //    }

    //    public async Task<ApplicationResponse<Setting>> DeleteSettingAsync(int id)
    //    {
    //        try
    //        {
    //            var setting = await _context.Settings.FindAsync(id);
    //            if (setting == null)
    //            {
    //                return ResponseFactory.CreateErrorResponse<Setting>(
    //                    "Setting not found.",
    //                    "Id",
    //                    "No setting found with the specified ID.",
    //                    StatusCodes.Status404NotFound);
    //            }

    //            _context.Settings.Remove(setting);
    //            await _context.SaveChangesAsync();

    //            return ResponseFactory.CreateSuccessResponse(
    //                setting,
    //                "Setting deleted successfully.",
    //                StatusCodes.Status200OK);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ResponseFactory.CreateErrorResponse<Setting>(
    //                "Setting deletion failed.",
    //                "Exception",
    //                ex.Message,
    //                StatusCodes.Status500InternalServerError);
    //        }
    //    }
    //}
}
