using Microsoft.AspNetCore.Mvc;
using Speech2Text.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Speech2Text.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriptsController : ControllerBase
    {
        // GET: api/<TranscriptsController>
        [HttpGet]
        public IEnumerable<Transcript> Get()
        {
            return new List<Transcript>();
        }

        // GET api/<TranscriptsController>/5
        [HttpGet("{id}")]
        public Transcript Get(int id)
        {
            return new Transcript();
        }

        // POST api/<TranscriptsController>
        [HttpPost]
        public IActionResult Post([FromBody] Transcript value)
        {
            return Accepted(new Transcript());
        }

        // PUT api/<TranscriptsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Transcript value)
        {
        }

        // DELETE api/<TranscriptsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
