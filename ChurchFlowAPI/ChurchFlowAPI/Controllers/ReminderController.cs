using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Models;
using ChurchFlowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChurchFlowAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }


        [HttpGet("schedule/{prayerScheduleId}")]
        public async Task<IActionResult> GetAll(int prayerScheduleId)
        {
            var reminders = await _reminderService.GetAllByPrayerScheduleAsync(prayerScheduleId);
            return Ok(reminders);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReminderCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var reminder = await _reminderService.CreateAsync(dto, userId);
            return Ok(reminder);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id);
            if (reminder == null) return NotFound();
            return Ok(reminder);
        }

        [Authorize]
        [HttpGet("my-reminder")]
        public async Task<IActionResult> GetMyReminders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var results = await _reminderService.GetMyRemindersAsync(userId);
            return Ok(results);
        }

    }

}
