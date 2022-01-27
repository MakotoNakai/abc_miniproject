using AbcMini1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AbcMini1.Controllers; 

[ApiController]
[Route("[controller]")]
public class DummyDataController : Controller {
    /// <summary>
    /// ダミーデータを作成
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateDummyData() {
        var start = DateTime.UtcNow - TimeSpan.FromMinutes(30);
        var col = Db.Instance.Collection("activities");
        var list = new List<Activity> {
            new("1", "walk", start, true),
            new("1", "walk", start + TimeSpan.FromSeconds(5), true),
            new("1", "sit", start + TimeSpan.FromSeconds(10), true),
            new("2", "walk", start, true),
            new("2", "stairs", start + TimeSpan.FromSeconds(5), true),
            new("2", "stairs", start + TimeSpan.FromSeconds(10), true),
            new("3", "walk", start, true),
            new("3", "stairs", start + TimeSpan.FromSeconds(5), true),
            new("3", "stairs", start + TimeSpan.FromSeconds(10), true),
            new("4", "walk", start, true),
            new("4", "stairs", start + TimeSpan.FromSeconds(5), true),
            new("4", "stairs", start + TimeSpan.FromSeconds(10), true),
            new("5", "walk", start, true),
            new("5", "walk", start + TimeSpan.FromSeconds(5), true),
            new("5", "sit", start + TimeSpan.FromSeconds(10), true),
            new("6", "stairs", start, true),
            new("6", "sit", start + TimeSpan.FromSeconds(5), true),
            new("6", "sit", start + TimeSpan.FromSeconds(10), true),
        };

        var batch = Db.Instance.StartBatch();
        foreach (var a in list) {
            batch.Create(col.Document(), a);
        }

        await batch.CommitAsync();
        return Ok();
    }
    
    /// <summary>
    /// ダミーデータを削除
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteDummyData() {
        var batch = Db.Instance.StartBatch();
        var query = Db.Instance.Collection("activities").WhereEqualTo("Dummy", true);
        var list = await query.GetSnapshotAsync();
        foreach (var ss in list) {
            batch.Delete(ss.Reference);
        }

        await batch.CommitAsync();
        
        return Ok();
    }
}