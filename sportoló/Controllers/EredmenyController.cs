using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sportoló.Models;

namespace sportoló.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        private readonly string Connectionstring = "Server = localhost; Database=sportolo13b;uid=root;Password=;";

        [HttpGet]
        //összes eredmény listázása
        public List<eredmeny> GetAllResoults()
        {
            var connector = new MySqlConnection(Connectionstring);
            connector.Open();
            string sql = "SELECT * FROM `eredmeny`";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                var eredmeny = new eredmeny
                {
                    id = dataReader.GetInt32(0),
                    Competition = dataReader.GetString(1),
                    Description = dataReader.GetString(2),
                    ResultTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    SportoloId = dataReader.GetInt32(5)
                };

            }
            connector.Close();
            return null;
        }
        [HttpGet]
        //(id alapján) – egy adott eredmény lekérdezése
        [HttpPost]
        //új eredmény rögzítése (a resultTime és az updateTime mező automatikusan az aktuális időpontra álljon be)
        [HttpPut]
        //meglévő eredmény módosítása (az updateTime mezőt minden módosításkor frissíteni kell)
        [HttpDelete]
        //eredmény törlése
    }
}
