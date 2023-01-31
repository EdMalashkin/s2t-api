using Microsoft.AspNetCore.Mvc;
using Speech2Text.Api.Models;
using Speech2Text.Api.Services;
using Speech2Text.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Speech2Text.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TranscriptsController : ControllerBase
    {
        private readonly ICosmosDbService<Transcript> _cosmosDbService;

        public TranscriptsController(ICosmosDbService<Transcript> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        }

        // GET: <TranscriptsController>
        [HttpGet]
        public async Task<IEnumerable<Transcript>> Get()
        {
            return await _cosmosDbService.GetMultipleAsync("select * from c");
        }

        // GET <TranscriptsController>/5
        [HttpGet("{id}")]
        public async Task<Transcript> Get(string id)
        {
            return await _cosmosDbService.GetAsync(id);
        }

        // POST <TranscriptsController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Transcript transcriptTask)
        {
            transcriptTask.Id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddAsync(transcriptTask);
            return StatusCode(StatusCodes.Status201Created, transcriptTask);
        }


        // PUT <TranscriptsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Transcript transcriptTask)
        {
            transcriptTask.Id = id; // to be sure
            await _cosmosDbService.UpdateAsync(id, transcriptTask);
            return Ok(transcriptTask);
        }

        // DELETE <TranscriptsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _cosmosDbService.DeleteAsync(id);
            return Ok();
        }

		// DELETE <TranscriptsController>
		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] Transcript templateTask)
		{
			var query = new TranscriptQuery(templateTask).ToString();
			var filteredTasks = await _cosmosDbService.GetMultipleAsync(query);
			foreach (var t in filteredTasks)
			{
				await _cosmosDbService.DeleteAsync(t.Id);
			}
			
			return Ok();
		}
	}
}

