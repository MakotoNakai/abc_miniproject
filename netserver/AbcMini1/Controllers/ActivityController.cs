using System.ComponentModel.DataAnnotations;
using AbcMini1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AbcMini1.Controllers; 

[ApiController]
[Route("[controller]")]
public class ActivityController : Controller {
    // GET
    [HttpGet]
    public async Task<IActionResult> Index() {
        var query = Db.Instance.Collection("activities")
            .WhereEqualTo("DeviceId", "3");
        var ss = await query.GetSnapshotAsync();
        return Ok();
    }
    
    /// <summary>
    /// （iOSアプリ用）行動データを記録します。
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ActivityPostPayload request) {
        if (!ModelState.IsValid) return BadRequest();
        var deviceDoc = Db.Instance.Document($"devices/{request.DeviceId}");
        var device = await deviceDoc.GetSnapshotAsync();
        if (!device.Exists) {
            await deviceDoc.SetAsync(new Device());
        }
        var col = Db.Instance.Collection("activities");
        foreach (var act in request.Activities) {
            var dt = DateTime.Parse(act.Timestamp);
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            await col.AddAsync(new Activity {
                DeviceId = request.DeviceId,
                Type = Enum.Parse<Activity.ActivityType>(act.Type),
                Timestamp = dt
            });
        }

        return Ok();
    }

    public class ActivityPostPayload {
        [Required] public string DeviceId { get; set; }
        [Required] public RawActivity[] Activities { get; set; }

        public class RawActivity {
            public string Timestamp { get; set; }
            public string Type { get; set; }
        }
    }
}