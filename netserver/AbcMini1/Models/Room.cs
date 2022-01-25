namespace AbcMini1.Models; 

public class Room {
    public List<List<string>> Groups { get; } = new();

    public static Room Create(int num, Dictionary<string, Activity[]> targets) {
        var score = new int[num, num];

        var first = new DateTime(9999, 1, 1);
        var last = new DateTime(0, 1, 1);
        foreach (var arr in targets.Values) {
            Array.Sort(arr);
            first = arr.Min(a => a.Timestamp);
        }

        var span = new TimeSpan(0, 0, 0, 5);
        for (var t = first; t < last; t += span) {
            
        }

        return new Room();
    }
}