using ChurchFlowAPI.Data;
using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurchFlowAPI.Services
{
    //interface
    public interface IParticipationService
    {
        Task<IEnumerable<Participation>> GetAllByUserAsync(string userId);
        Task<IEnumerable<Participation>> GetAllByPrayerScheduleAsync(int prayerScheduleId);
        Task<ParticipationDto> CreateAsync(ParticipationCreateDto dto, string userId);
        Task<IEnumerable<ParticipationDto>> GetMyParticipationsAsync(string userId);

    }

    //logic
    public class ParticipationService : IParticipationService
    {
        private readonly ApplicationDbContext _context;

        public ParticipationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Participation>> GetAllByUserAsync(string userId)
        {
            return await _context.Participations
                .Where(p => p.UserId == userId)
                .Include(p => p.PrayerSchedule)
                .ToListAsync();
        }

        public async Task<IEnumerable<Participation>> GetAllByPrayerScheduleAsync(int prayerScheduleId)
        {
            return await _context.Participations
                .Where(p => p.PrayerScheduleId == prayerScheduleId)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<ParticipationDto> CreateAsync(ParticipationCreateDto dto, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var schedule = await _context.PrayerSchedules.FindAsync(dto.PrayerScheduleId);

            var participation = new Participation
            {
                UserId = userId,
                PrayerScheduleId = dto.PrayerScheduleId,
                IsParticipating = dto.IsParticipating,
                CreatedById = userId
            };

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return new ParticipationDto
            {
                UserId = userId,
                UserFullName = user?.FullName,
                PrayerScheduleId = schedule.Id,
                PrayerScheduleTitle = schedule?.Title,
                IsParticipating = dto.IsParticipating,
                CreatedById = userId,
                CreatedByFullName = user?.FullName
            };
        }



        public async Task<IEnumerable<ParticipationDto>> GetMyParticipationsAsync(string userId)
        {
            var participations = await _context.Participations
                .Where(p => p.UserId == userId)
                .Include(p => p.PrayerSchedule)
                .Include(p => p.User)
                .Include(p => p.CreatedBy)
                .ToListAsync();

            return participations.Select(p => new ParticipationDto
            {
                UserId = p.UserId,
                UserFullName = p.User.FullName, // from ApplicationUser

                PrayerScheduleId = p.PrayerScheduleId,
                PrayerScheduleTitle = p.PrayerSchedule?.Title,
                PrayerDate = p.PrayerSchedule.ScheduledAt,

                IsParticipating = p.IsParticipating,

                CreatedById = p.CreatedById,
                CreatedByFullName = p.CreatedBy?.FullName
            });
        }


    }

}
