using Microsoft.AspNetCore.Mvc;
using Speech2Text.Core.Models;
using Speech2Text.Core.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Speech2Text.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private const string containerName = "tasks";
        private readonly ICosmosDbService<TranscriptTask> _cosmosDbService;
        public TasksController(CosmosDBSettings cosmosDBSettings)
        {
            if (cosmosDBSettings == null) throw new ArgumentNullException(nameof(cosmosDBSettings));
            var builder = new CosmosDbServiceBuilder<TranscriptTask>(cosmosDBSettings);
            _cosmosDbService = builder.GetCosmosDbService(containerName);
        }

        // GET: <TasksController>
        [HttpGet]
        public async Task<IEnumerable<TranscriptTask>> Get()
        {
            return await _cosmosDbService.GetMultipleAsync("select * from c");
        }

        // GET <TasksController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _cosmosDbService.GetAsync(id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST <TasksController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TranscriptTask transcriptTask)
        {
            transcriptTask.Id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddAsync(transcriptTask.Id, transcriptTask);
            return StatusCode(StatusCodes.Status201Created, transcriptTask);
        }


        // PUT <TasksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] TranscriptTask transcriptTask)
        {
            transcriptTask.Id = id; // to be sure
            await _cosmosDbService.UpdateAsync(id, transcriptTask);
            return Ok(transcriptTask);
        }

        // DELETE <TasksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _cosmosDbService.DeleteAsync(id);
            return Ok();
        }

        // DELETE <TasksController>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] TranscriptTask template)
        {
            var query = new TranscriptQuery(template).ToString();
            var filteredTasks = await _cosmosDbService.GetMultipleAsync(query);
            foreach (var t in filteredTasks)
            {
                await _cosmosDbService.DeleteAsync(t.Id);
            }

            return Ok();
        }
    }
}

