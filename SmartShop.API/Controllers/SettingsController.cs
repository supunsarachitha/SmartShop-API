using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;

namespace SmartShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly ISettingsService _settingsService;

        public SettingsController(ILogger<SettingsController> logger, ISettingsService settingsService)
        {
            _logger = logger;
            _settingsService = settingsService;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var response = await _settingsService.GetAllSettingsAsync();
            if (response.StatusCode == 200 || response.StatusCode == null)
                return Ok(response);
            return StatusCode(response.StatusCode.Value, response);
        }

        // GET: api/Settings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSetting(Guid id)
        {
            var response = await _settingsService.GetSettingByIdAsync(id);

            if (response.StatusCode == 200 || response.StatusCode == null)
                return Ok(response);

            return StatusCode(response.StatusCode.Value, response);
        }

        // PUT: api/Settings/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutSetting(Guid id, Setting setting)
        {
            if (id != setting.Id)
                return BadRequest("Id mismatch.");

            var response = await _settingsService.UpdateSettingAsync(id, setting);
            if (response.StatusCode == 200 || response.StatusCode == null)
                return Ok(response);
            if (response.StatusCode == 400)
                return BadRequest(response);
            return StatusCode(response.StatusCode.Value, response);
        }

        // POST: api/Settings
        [HttpPost]
        public async Task<IActionResult> PostSetting(Setting setting)
        {
            var response = await _settingsService.CreateSettingAsync(setting);

            if (!response.Success)
                return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);

            return CreatedAtAction(nameof(GetSetting), new { id = response.Data.Id }, response);
        }


        // DELETE: api/Settings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSetting(Guid id)
        {
            var response = await _settingsService.DeleteSettingAsync(id);
            if (response.Success)
                return Ok(response);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }
    }
}
