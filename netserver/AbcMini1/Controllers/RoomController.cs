using System.ComponentModel.DataAnnotations;
using AbcMini1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AbcMini1.Controllers; 

[ApiController]
[Route("[controller]")]
public class RoomController : Controller {
    
    /// <summary>
    /// （Webアプリ用）グループ分けを実行します。
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json", Type = typeof(Room))]
    public async Task<IActionResult> Create([FromBody] CreatePayload payload) {
        var targets = new Dictionary<string, Activity[]>();
        foreach (var deviceId in payload.DeviceIds) {
            var query = Db.Instance.Collection("activities")
                .WhereEqualTo("deviceId", deviceId);
            var ss = await query.GetSnapshotAsync();
            targets.Add(deviceId, ss.Documents.Select(x => x.ConvertTo<Activity>()).ToArray());
        }
        
        return Ok(Room.Create(payload.TeamNumber, targets));
    }
    
    public class CreatePayload {
        /// <summary>
        /// グループ分けを行う対象デバイスIDのリスト
        /// </summary>
        [Required] public string[] DeviceIds { get; set; }
        
        /// <summary>
        /// 分けるグループの数
        /// 3を指定すると3グループ作られます
        /// </summary>
        [Required] public int TeamNumber { get; set; }
    }

    
}