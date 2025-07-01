using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChurchFlowAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipationController : ControllerBase
    {
        private readonly IParticipationService _participationService;

        public ParticipationController(IParticipationService participationService)
        {
            _participationService = participationService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(string userId)
        {
            var participations = await _participationService.GetAllByUserAsync(userId);
            return Ok(participations);
        }

        [Authorize]
        [HttpGet("my-participation")]
        public async Task<IActionResult> GetMyParticipations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var results = await _participationService.GetMyParticipationsAsync(userId);
            return Ok(results);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParticipationCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _participationService.CreateAsync(dto, userId);
            return Ok(result);
        }



        [Authorize(Roles = "ADMIN")]
        [HttpGet("schedule/{prayerScheduleId}")]
        public async Task<IActionResult> GetByPrayerSchedule(int prayerScheduleId)
        {
            var participation = await _participationService.GetAllByPrayerScheduleAsync(prayerScheduleId);
            if (participation == null) return NotFound();
            return Ok(participation);
        }
    }

}
