using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyThings.Controllers
{
    [Route("api/[controller]")]
    public class MyThingsController : ControllerBase
    {
        // GET api/mythings
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var list = new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }).ToList());
            //var list = new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            return list;
        }

       // GET api/mythings/5
       // [Authorize]
       // [HttpGet("{id}")]
       // public IActionResult Get(string id)
       // {
       //     var item = new JsonResult(User.Claims.Where(c => c.Subject.ToString() == id).ToList());
       //     return item;
       // }

       // POST api/mythings
       //[HttpPost]
       // public void Post([FromBody]string value)
       // {
       // }

       // PUT api/mythings/5
       // [HttpPut("{id}")]
       // public void Put(int id, [FromBody]string value)
       // {
       // }

       // DELETE api/mythings/5
       // [HttpDelete("{id}")]
       // public void Delete(int id)
       // {
       // }

    }
}
