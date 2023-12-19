﻿using Microsoft.AspNetCore.Mvc;
using Speech2Text.Core;
using Speech2Text.Core.Models;
using Speech2Text.Core.Services;

namespace Speech2Text.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TranscriptsController : ControllerBase
    {
        private TasksController tasks;
        private YoutubeTranscriptsController youtubeTranscripts;
        private Dictionary<string, string> quickChartSettings;

        public TranscriptsController(CosmosDBSettings cosmosDBSettings, Dictionary<string, string> quickChartSettings)
        {
			this.tasks = new TasksController(cosmosDBSettings);
            this.youtubeTranscripts = new YoutubeTranscriptsController(cosmosDBSettings);
            this.quickChartSettings = quickChartSettings;
        }

        // GET: <TranscriptsController>
        [HttpGet]
        public async Task<IEnumerable<Transcript>> Get()
        {
            return await youtubeTranscripts.Get();
        }

        // GET: <TranscriptsController>
        [HttpPost("bytemplate")]
        public async Task<IEnumerable<Transcript>> Get([FromBody] TranscriptTask template)
        {
            return await youtubeTranscripts.Get(template);
        }

        // GET <TranscriptsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await youtubeTranscripts.Get(id);
        }

        // GET <TranscriptsController>/5/chart
        [HttpGet("{id}/chart")]
        public async Task<IActionResult> GetChart(string id)
        {
            Stream? result = null;
			string? text = await GetTranscriptDetails(id, "lemmatized");
			if (text != null)
			{
				var chart = new Chart(text, this.quickChartSettings);
				result = await chart.GetPng();
			}
            if(result != null)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }


		// GET <TranscriptsController>/5/text
		[HttpGet("{id}/{textType}")]
		public async Task<IActionResult> GetText(string id, string textType)
		{
			string? result = await GetTranscriptDetails(id, textType);
			if (result != null)
			{
				return StatusCode(StatusCodes.Status200OK, result);
			}
			else
			{
				return StatusCode(StatusCodes.Status404NotFound);
			}
		}


		private async Task<string?> GetTranscriptDetails(string id, string attrName = "text")
		{
			string? result = null;
			var actionResult = await youtubeTranscripts.Get(id);
			var okResult = actionResult as ObjectResult;
			if (okResult != null && okResult.Value != null)
			{
				var transcript = okResult.Value as Transcript;
				if (transcript != null && transcript.Data != null)
				{
					result = string.Join(" ", transcript.Data.Select(j => j.Value<string>(attrName)));
				}
			}
			return result;
		}

		// POST <TranscriptsController>
		[HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TranscriptTask transcriptTask)
        {
            // commented as now we have this logic at bot's client side
            //var existing = await youtubeTranscripts.Get(transcriptTask);
            //if (existing.Any()) {// optimization: return old if exists
            //    return StatusCode(StatusCodes.Status200OK, existing.LastOrDefault());
            //}
            //else
            //{
            //    return await tasks.PostAsync(transcriptTask);
            //}
            return await tasks.PostAsync(transcriptTask);
        }


        // PUT <TranscriptsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] TranscriptTask transcriptTask)
        {
            return await tasks.Put(id, transcriptTask);
        }

        // DELETE <TranscriptsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await youtubeTranscripts.Delete(id);
        }

		// DELETE <TranscriptsController>
		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] Transcript template)
		{
            return await youtubeTranscripts.Delete(template);
        }
	}
}

