namespace AbcMini1.Models; 

public class DeviceActivity {
    public string DeviceId { get; set; }
    public SortedSet<DateTime> Timestamps { get; set; }
    public Dictionary<DateTime, Activity> Activities { get; set; }

    public DeviceActivity(string deviceId, Activity[] activities) {
        DeviceId = deviceId;
        Timestamps = new SortedSet<DateTime>(activities.Select(a => a.Timestamp));
        Activities = activities.ToDictionary(a => a.Timestamp, a => a);
    }
}