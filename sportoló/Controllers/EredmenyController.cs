using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sportoló.Models;

namespace sportoló.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        private readonly string Connectionstring = "Server=localhost;Database=sportolo13b;uid=root;Password=;";

        // összes eredmény listázása
        [HttpGet]
        public ActionResult<List<eredmeny>> GetAllResults()
        {
            var results = new List<eredmeny>();
            using var connector = new MySqlConnection(Connectionstring);
            connector.Open();
            const string sql = "SELECT id, Competition, Description, ResultTime, UpdateTime, SportoloId FROM eredmeny";
            using var cmd = new MySqlCommand(sql, connector);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new eredmeny
                {
                    id = reader.GetInt32(0),
                    Competition = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    ResultTime = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                    UpdateTime = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                    SportoloId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                });
            }

            return Ok(results);
        }

        // (id alapján) – egy adott eredmény lekérdezése
        [HttpGet("{id}")]
        public ActionResult<eredmeny> GetById(int id)
        {
            using var connector = new MySqlConnection(Connectionstring);
            connector.Open();
            const string sql = "SELECT id, Competition, Description, ResultTime, UpdateTime, SportoloId FROM eredmeny WHERE id = @id";
            using var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return NotFound();

            var item = new eredmeny
            {
                id = reader.GetInt32(0),
                Competition = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                ResultTime = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                UpdateTime = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                SportoloId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
            };

            return Ok(item);
        }

        // új eredmény rögzítése (a ResultTime és az UpdateTime mező automatikusan az aktuális időpontra álljon be)
        [HttpPost]
        public ActionResult<eredmeny> Create([FromBody] eredmeny value)
        {
            if (value == null) return BadRequest();

            var now = DateTime.Now;
            using var connector = new MySqlConnection(Connectionstring);
            connector.Open();
            const string sql = "INSERT INTO eredmeny (Competition, Description, ResultTime, UpdateTime, SportoloId) VALUES (@competition, @description, @resultTime, @updateTime, @sportoloId)";
            using var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@competition", value.Competition ?? string.Empty);
            cmd.Parameters.AddWithValue("@description", value.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@resultTime", now);
            cmd.Parameters.AddWithValue("@updateTime", now);
            cmd.Parameters.AddWithValue("@sportoloId", value.SportoloId);
            cmd.ExecuteNonQuery();
            var insertedId = (int)cmd.LastInsertedId;

            value.id = insertedId;
            value.ResultTime = now;
            value.UpdateTime = now;

            return CreatedAtAction(nameof(GetById), new { id = insertedId }, value);
        }

        // meglévő eredmény módosítása (az UpdateTime mezőt minden módosításkor frissíteni kell)
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] eredmeny value)
        {
            if (value == null || id != value.id) return BadRequest();

            var now = DateTime.Now;
            using var connector = new MySqlConnection(Connectionstring);
            connector.Open();
            const string sql = "UPDATE eredmeny SET Competition=@competition, Description=@description, ResultTime=@resultTime, UpdateTime=@updateTime, SportoloId=@sportoloId WHERE id=@id";
            using var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@competition", value.Competition ?? string.Empty);
            cmd.Parameters.AddWithValue("@description", value.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@resultTime", value.ResultTime);
            cmd.Parameters.AddWithValue("@updateTime", now);
            cmd.Parameters.AddWithValue("@sportoloId", value.SportoloId);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0) return NotFound();

            return NoContent();
        }

        // eredmény törlése
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connector = new MySqlConnection(Connectionstring);
            connector.Open();
            const string sql = "DELETE FROM eredmeny WHERE id = @id";
            using var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0) return NotFound();

            return NoContent();
        }
    }
}
