﻿using Microsoft.AspNetCore.Mvc;
using Speech2Text.Core.Models;
using Speech2Text.Core.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Speech2Text.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class YoutubeTranscriptsController : ControllerBase
	{
		private const string containerName = "youtubeTranscripts";
		private readonly ICosmosDbService<Transcript> _cosmosDbService;
		public YoutubeTranscriptsController(CosmosDBSettings cosmosDBSettings)
		{
			if (cosmosDBSettings == null) throw new ArgumentNullException(nameof(cosmosDBSettings));
			var builder = new CosmosDbServiceBuilder<Transcript>(cosmosDBSettings);
			_cosmosDbService = builder.GetCosmosDbService(containerName);
		}

		// GET: <YoutubeTranscriptsController>
		[HttpGet]
		public async Task<IEnumerable<String>> Get()
		{
			var result = await _cosmosDbService.GetMultipleAsync("select c.id from c");
			return result.Select(t => t.Id);
		}


        [HttpPost]
        public async Task<IEnumerable<Transcript>> Get([FromBody] TranscriptTask template)
        {
            var query = new TranscriptQuery(template).ToString();
            return await _cosmosDbService.GetMultipleAsync(query);
        }

        // GET <YoutubeTranscriptsController>/5
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

		// GET: <YoutubeTranscriptsController>/getIdByMediaId/{mediaId}
		[HttpGet("getIdByMediaId/{mediaId}")]
		public async Task<IActionResult> GetIdByMediaId(string mediaId)
		{ // get successful transcripts only!!!
			try
			{   //SELECT * FROM c WHERE c.OriginalURL LIKE '%j7JsuOmqheY%' AND IS_DEFINED(c.Data) ORDER BY c._ts DESC -- script to run in Azure Cosmos DB with capital letters
				var result = await _cosmosDbService.GetMultipleAsync($"SELECT top 1 c.id FROM c WHERE c.originalURL LIKE '%{mediaId}%' AND IS_DEFINED(c.data) ORDER BY c._ts DESC"); // todo: optimize search architecture
				return StatusCode(StatusCodes.Status200OK, result?.FirstOrDefault()?.Id);
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

        internal async Task<Transcript> GetTranscript(string id)
		{
			return await _cosmosDbService.GetAsync(id);
		}

		// POST <YoutubeTranscriptsController>
		[HttpPost("{id}")]
		public async Task<IActionResult> PostAsync(string id, [FromBody] Transcript json)
		{
			await _cosmosDbService.AddAsync(id, json);
			return StatusCode(StatusCodes.Status201Created, json);
		}


		// PUT <YoutubeTranscriptsController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] Transcript json)
		{
			await _cosmosDbService.UpdateAsync(id, json);
			return Ok(json);
		}

		// DELETE <YoutubeTranscriptsController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			await _cosmosDbService.DeleteAsync(id);
			return Ok();
		}

        // DELETE <YoutubeTranscriptsController>/5
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] TranscriptTask template)
        {
            var filteredTasks = await this.Get(template);
            foreach (var t in filteredTasks)
            {
                await _cosmosDbService.DeleteAsync(t.Id);
            }
            return Ok();
        }
    }
}
