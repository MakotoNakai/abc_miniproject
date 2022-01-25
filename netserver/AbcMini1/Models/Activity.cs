using Google.Cloud.Firestore;

namespace AbcMini1.Models;

[FirestoreData]
public class Activity : IComparable<Activity> {
    [FirestoreProperty] public ActivityType Type { get; set; }
    [FirestoreProperty] public DateTime Timestamp { get; set; }
    [FirestoreProperty] public string DeviceId { get; set; }
    
    [FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<ActivityType>))]
    public enum ActivityType {
        Sit,
        Walk,
        Stairs,
        Squat
    }

    public int CompareTo(Activity? other) {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Timestamp.CompareTo(other.Timestamp);
    }

    public int Similarity(Activity other) {
        // todo
        var score = 0;
        if (Type == other.Type) score += 1;
        return score;
    }
}