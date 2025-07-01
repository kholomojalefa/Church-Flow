using ChurchFlowAPI.Data;
using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurchFlowAPI.Services
{
    //interface
    public interface IPrayerScheduleService
    {
        Task<IEnumerable<PrayerScheduleDto>> GetAllAsync();
        Task<PrayerSchedule> CreateAsync(PrayerScheduleCreateDto dto);
        Task<IEnumerable<PrayerScheduleDto>> GetMySchedulesAsync(string userId);
        Task<PrayerSchedule> GetByIdAsync(int id);
    }

    //logic
    public class PrayerScheduleService : IPrayerScheduleService
    {
        private readonly ApplicationDbContext _context;

        public PrayerScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrayerScheduleDto>> GetAllAsync()
        {
            var schedules = await _context.PrayerSchedules
                .Include(s => s.Participations)
                .ToListAsync();

            return schedules.Select(ps => new PrayerScheduleDto
            {
                Id = ps.Id,
                Title = ps.Title,
                Description = ps.Description,
                ScheduledAt = ps.ScheduledAt,
                ParticipantCount = ps.Participations.Count
            });
        }

        public async Task<PrayerSchedule> CreateAsync(PrayerScheduleCreateDto dto)
        {
            var schedule = new PrayerSchedule
            {
                Title = dto.Title,
                Description = dto.Description,
                ScheduledAt = dto.ScheduledAt
            };

            _context.PrayerSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }


        public async Task<PrayerSchedule> GetByIdAsync(int id)
        {
            return await _context.PrayerSchedules.FindAsync(id);
        }

        public async Task<IEnumerable<PrayerScheduleDto>> GetMySchedulesAsync(string userId)
        {
            var schedules = await _context.PrayerSchedules
                .Where(s => s.Participations.Any(p => p.UserId == userId))
                .Include(s => s.Participations)
                .ToListAsync();

            return schedules.Select(ps => new PrayerScheduleDto
            {
                Id = ps.Id,
                Title = ps.Title,
                Description = ps.Description,
                ScheduledAt = ps.ScheduledAt,
                ParticipantCount = ps.Participations.Count
            });
        }



    }

}
