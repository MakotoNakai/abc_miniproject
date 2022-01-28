using System;
using System.Collections.Generic;
using AbcMini1.Models;
using NUnit.Framework;

namespace AbcMini1.Test;

public class RoomTests {
    private static Dictionary<string, Activity[]> TestData;
    [SetUp]
    public void Setup() {
        var start = DateTime.Now - TimeSpan.FromMinutes(30);
        TestData = new Dictionary<string, Activity[]> {
            {"1", new Activity[] {
                new("1", "walk", start),
                new("1", "walk", start + TimeSpan.FromSeconds(5)),
                new("1", "sit", start + TimeSpan.FromSeconds(10)),
            }},
            {"2", new Activity[] {
                new("2", "walk", start),
                new("2", "stairs", start + TimeSpan.FromSeconds(5)),
                new("2", "stairs", start + TimeSpan.FromSeconds(10)),
            }},
            {"3", new Activity[] {
                new("3", "walk", start),
                new("3", "stairs", start + TimeSpan.FromSeconds(5)),
                new("3", "stairs", start + TimeSpan.FromSeconds(10)),
            }},
            {"4", new Activity[] {
                new("4", "walk", start),
                new("4", "stairs", start + TimeSpan.FromSeconds(5)),
                new("4", "stairs", start + TimeSpan.FromSeconds(10)),
            }},
            {"5", new Activity[] {
                new("5", "walk", start),
                new("5", "walk", start + TimeSpan.FromSeconds(5)),
                new("5", "sit", start + TimeSpan.FromSeconds(10)),
            }},
            {"6", new Activity[] {
                new("6", "stairs", start),
                new("6", "sit", start + TimeSpan.FromSeconds(5)),
                new("6", "sit", start + TimeSpan.FromSeconds(10)),
            }}
        };
    }

    [Test]
    public void TestScore() {
        var results = Room.Nodes(TestData, TimeSpan.FromHours(1));
        Assert.AreEqual(1, results["1"].Scores[results["2"]]);
        Assert.AreEqual(1, results["2"].Scores[results["1"]]);
        Assert.AreEqual(3, results["2"].Scores[results["3"]]);
    }

    [Test]
    public void TestCut() {
        var nodes = Room.Nodes(TestData, TimeSpan.FromHours(1));
        var results = Room.Cut(new Queue<int>(new[]{3, 3}), new HashSet<Room.Node>(nodes.Values));
    }
}