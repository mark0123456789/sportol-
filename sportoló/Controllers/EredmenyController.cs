using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sportoló.Models;

namespace sportoló.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        private readonly string connectionstring = "Server = localhost; Database=sportolo13b;uid=root;Password=;" ;

        [HttpGet]
        //összes eredmény listázása
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
