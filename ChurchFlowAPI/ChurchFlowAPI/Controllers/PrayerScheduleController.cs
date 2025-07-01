using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChurchFlowAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrayerScheduleController : ControllerBase
    {
        private readonly IPrayerScheduleService _prayerScheduleService;

        public PrayerScheduleController(IPrayerScheduleService prayerScheduleService)
        {
            _prayerScheduleService = prayerScheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _prayerScheduleService.GetAllAsync();
            return Ok(schedules);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrayerScheduleCreateDto dto)
        {
            var schedule = await _prayerScheduleService.CreateAsync(dto);
            return Ok(schedule);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _prayerScheduleService.GetByIdAsync(id);
            if (schedule == null)
                return NotFound();

            return Ok(schedule);
        }

        [HttpGet("my-schedule")]
        public async Task<IActionResult> GetMyPrayerSchedules()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var results = await _prayerScheduleService.GetMySchedulesAsync(userId);
            return Ok(results);
        }


    }

}
