using Microsoft.AspNetCore.Mvc;
using Speech2Text.Api.Services;
using Speech2Text.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Speech2Text.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriptsController : ControllerBase
    {
        private readonly ICosmosDbService<Transcript> _cosmosDbService;

        public TranscriptsController(ICosmosDbService<Transcript> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        }

        // GET: api/<TranscriptsController>
        [HttpGet]
        public async Task<IEnumerable<Transcript>> Get()
        {
            return await _cosmosDbService.GetMultipleAsync("select * from c");
        }

        // GET api/<TranscriptsController>/5
        [HttpGet("{id}")]
        public async Task<Transcript> Get(string id)
        {
            return await _cosmosDbService.GetAsync(id);
        }

        // POST api/<TranscriptsController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Transcript transcriptTask)
        {
            transcriptTask.Id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddAsync(transcriptTask);
            return Ok(transcriptTask);
        }


        // PUT api/<TranscriptsController>/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] Transcript value)
        {
            value.Id = id; // to be sure
            await _cosmosDbService.UpdateAsync(id, value);
        }

        // DELETE api/<TranscriptsController>/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _cosmosDbService.DeleteAsync(id);
        }
    }
}
