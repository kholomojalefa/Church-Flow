namespace ChurchFlowAPI.DTOs
{
    // ReminderCreateDto.cs
public class ReminderCreateDto
{
    public string Message { get; set; }
    public DateTime SendAt { get; set; }
    public DateTime PrayerScheduleDate { get; set; }
    public int PrayerScheduleId { get; set; }
}

// ReminderDto.cs (for return data)
public class ReminderDto : ReminderCreateDto
{
    public string CreatedById { get; set; }
    public string CreatedByName { get; set; }
    public string PrayerScheduleTitle { get; set; }
}

}
