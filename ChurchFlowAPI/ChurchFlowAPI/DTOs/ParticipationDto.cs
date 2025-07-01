namespace ChurchFlowAPI.DTOs
{
    public class ParticipationCreateDto
    {
        public string UserId { get; set; }
        public int PrayerScheduleId { get; set; }
        public bool IsParticipating { get; set; }
    }

    public class ParticipationDto
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }

        public int PrayerScheduleId { get; set; }
        public string PrayerScheduleTitle { get; set; } 
        public DateTime PrayerDate { get; set; }

        public bool IsParticipating { get; set; }

        public string CreatedById { get; set; }
        public string CreatedByFullName { get; set; }   // for admin tracking
    }

}
