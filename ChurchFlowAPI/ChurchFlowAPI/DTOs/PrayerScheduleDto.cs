namespace ChurchFlowAPI.DTOs
{
    public class PrayerScheduleCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ScheduledAt { get; set; }
    }

    public class PrayerScheduleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int ParticipantCount { get; set; }
    }
}
