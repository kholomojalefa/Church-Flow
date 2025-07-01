namespace ChurchFlowAPI.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SendAt { get; set; }
        public int PrayerScheduleId { get; set; }
        public PrayerSchedule PrayerSchedule { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

    }
}
