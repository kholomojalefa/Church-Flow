using ChurchFlowAPI.Data;
using ChurchFlowAPI.DTOs;
using ChurchFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurchFlowAPI.Services
{
    //interface
    public interface IReminderService
    {
        Task<IEnumerable<Reminder>> GetAllByPrayerScheduleAsync(int prayerScheduleId);
        Task<ReminderDto> CreateAsync(ReminderCreateDto dto, string userId);
        Task<Reminder> GetByIdAsync(int id);
        Task<IEnumerable<ReminderDto>> GetMyRemindersAsync(string userId);


    }

    //logic
    public class ReminderService : IReminderService
    {
        private readonly ApplicationDbContext _context;

        public ReminderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reminder>> GetAllByPrayerScheduleAsync(int prayerScheduleId)
        {
            return await _context.Reminders
                .Where(r => r.PrayerScheduleId == prayerScheduleId)
                .ToListAsync();
        }

        public async Task<ReminderDto> CreateAsync(ReminderCreateDto dto, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var prayerSchedule = await _context.PrayerSchedules.FindAsync(dto.PrayerScheduleId);

            var reminder = new Reminder
            {
                Message = dto.Message,
                SendAt = dto.SendAt,
                PrayerScheduleId = dto.PrayerScheduleId,
                CreatedById = userId
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            return new ReminderDto
            {
                Message = reminder.Message,
                SendAt = reminder.SendAt,
                PrayerScheduleId = reminder.PrayerScheduleId,
                CreatedById = userId,
                CreatedByName = user?.FullName,
                PrayerScheduleTitle = prayerSchedule?.Title
            };
        }

        public async Task<Reminder> GetByIdAsync(int id)
        {
            return await _context.Reminders.FindAsync(id);
        }

        public async Task<IEnumerable<ReminderDto>> GetMyRemindersAsync(string userId)
        {
            var reminders = await _context.Reminders
                .Include(r => r.CreatedBy)
                .Include(r => r.PrayerSchedule) // include related schedule
                .Where(r => r.CreatedById == userId)
                .ToListAsync();

            return reminders.Select(r => new ReminderDto
            {
                Message = r.Message,
                SendAt = r.SendAt,
                PrayerScheduleId = r.PrayerScheduleId,
                PrayerScheduleTitle = r.PrayerSchedule?.Title,
                PrayerScheduleDate = r.PrayerSchedule.ScheduledAt,
                CreatedById = r.CreatedById,
                CreatedByName = r.CreatedBy?.FullName
            });
        }



    }

}
