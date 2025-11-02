using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using R1API.Models;

namespace R1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindController : ControllerBase
    {
        //api/bind?lang?23651263561253761253125&latit=72637812637123784928 post
        //[HttpPost]
        //public IActionResult Loc(Location location)
        //{
        //    return Ok();
        //}
        [HttpPost("{Name}/{ManagerName}")]
        public IActionResult add([FromBody]string Name,[FromRoute]Department dept)//boddy : Contain more the onething
        {
            return Ok();
        }

        [HttpGet("{Latitute}/{Lang}")]
        public IActionResult GetPlace([FromRoute]Location loc)
        {
            return Ok();
        }
    }
}
