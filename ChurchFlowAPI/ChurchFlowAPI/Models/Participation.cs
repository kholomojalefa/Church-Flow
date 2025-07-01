namespace ChurchFlowAPI.Models
{
    public class Participation
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PrayerScheduleId { get; set; }
        public PrayerSchedule PrayerSchedule { get; set; }

        public bool IsParticipating { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}
