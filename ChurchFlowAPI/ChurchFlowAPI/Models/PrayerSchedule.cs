namespace ChurchFlowAPI.Models
{
    public class PrayerSchedule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ScheduledAt { get; set; }
        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();
        public ICollection<Reminder> Reminders { get; set; } = new HashSet<Reminder>();
    }
}
