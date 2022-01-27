namespace AbcMini1.Models;

public class Room {
    public List<List<string>> Groups { get; } = new();

    public Room(int num, Dictionary<string, Activity[]> targets, TimeSpan duration) {
        var nodes = Nodes(targets, duration);
        var groups = Cut(num, new HashSet<Node>(nodes.Values));

        Groups.AddRange(groups.Select(c => c.Nodes.Select(n => n.DeviceId).ToList()));
    }

    public static Dictionary<string, Node> Nodes(Dictionary<string, Activity[]> targets, TimeSpan duration) {
        var nodes = new Dictionary<string, Node>();
        foreach (var (name, a) in targets) {
            var n = new Node(name, a);
            nodes.Add(name, n);
        }

        foreach (var n1 in nodes.Values) {
            foreach (var n2 in nodes.Values) {
                if (n1 != n2) n1.Scores.Add(n2, 0);
            }
        }
        var last = DateTime.UtcNow;
        var first = last - duration;
        var activities = targets
            .ToDictionary(kv => kv.Key, kv => new DeviceActivity(kv.Key, kv.Value));

        var span = new TimeSpan(0, 0, 0, 5);
        for (var t = first; t < last; t += span) {
            // TODO kuso code

            foreach (var (deviceId, da) in activities) {
                var view = da.Timestamps.GetViewBetween(t, t + span);
                if (view.Count == 0) continue;
                var kind = da.Activities[view.Min].Type.ToLowerInvariant();
                if (kind == "sit") continue;

                foreach (var (deviceId2, da2) in activities) {
                    if (deviceId == deviceId2) break;
                    var view2 = da.Timestamps.GetViewBetween(t, t + span);
                    if (view2.Count == 0) continue;
                    var kind2 = da2.Activities[view2.Min].Type.ToLowerInvariant();
                    if (kind2 == "sit") continue;

                    if (kind == kind2) {
                        nodes[deviceId].Scores[nodes[deviceId2]] += 1;
                        nodes[deviceId2].Scores[nodes[deviceId]] += 1;
                    }
                }
            }
        }

        return nodes;
    }

    public static List<Cluster> Cut(int count, HashSet<Node> remaining, List<Cluster>? clusters = null) {
        var c = new Cluster();
        var origin = remaining.ElementAt(Random.Shared.Next(remaining.Count));
        for (var i = 0; i < count; i++) {
            if (remaining.Count == 0) break;

            remaining.Remove(origin);
            c.Nodes.Add(origin);

            var seq = origin.Scores.Where(kv => remaining.Contains(kv.Key)).OrderByDescending(kv => kv.Value);
            if (!seq.Any()) break;
            origin = seq.First().Key;
        }
        clusters ??= new List<Cluster>();
        clusters.Add(c);
        if (remaining.Count != 0) Cut(count, remaining, clusters);

        return clusters;
    }

    public class Cluster {
        public List<Node> Nodes { get; } = new();

        public override string ToString() {
            return string.Join(", ", Nodes.Select(n => n.ToString()));
        }
    }

    public class Node {
        public string DeviceId { get; }
        public Activity[] Activities { get; }
        public Dictionary<Node, int> Scores { get; }

        public Node(string deviceId, Activity[] activities) {
            DeviceId = deviceId;
            Activities = activities;
            Scores = new Dictionary<Node, int>();
        }

        public override string ToString() {
            return DeviceId;
        }
    }
}