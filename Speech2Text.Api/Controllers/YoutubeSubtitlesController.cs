using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Speech2Text.Core.Models;

namespace Speech2Text.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class YoutubeSubtitlesController : ControllerBase
	{
		private readonly HttpClient _httpClient;

		public YoutubeSubtitlesController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		// GET <YoutubeSubtitlesController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var url = $"https://speech2text-processing.azurewebsites.net/api/GetYoutubeTranscript?v={id}";
			var response = await _httpClient.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				//var transcript = await response.Content.ReadFromJsonAsync<Transcript>(); // it cannot convert JArray to Transcript
				var jsonResponse = await response.Content.ReadAsStringAsync();
				var transcript = JsonConvert.DeserializeObject<Transcript>(jsonResponse);
				if (transcript != null)
				{
					return Ok(transcript);
				}
				else
				{
					return Problem("Failed to deserialize the transcript.");
				}
			}

			return StatusCode((int)response.StatusCode, response.ReasonPhrase);
		}

		[HttpGet("{id}/stats")]
		public async Task<IActionResult> GetStats(string id, [FromQuery] int minfreq = 2)
		{
			var transcriptResult = await Get(id) as OkObjectResult;
			if (transcriptResult != null)
			{
				var transcript = transcriptResult.Value as Transcript;
				if (transcript == null)
				{
					return StatusCode(StatusCodes.Status404NotFound, new { message = "Transcript not found." });
				}
				else if (transcript.Error != null)
				{
					return StatusCode(StatusCodes.Status404NotFound, new { message = transcript.Error });
				}
				var s = new TokenStats(transcript);
				return StatusCode(StatusCodes.Status200OK, s.GetTopics(minfreq));
			}
			return StatusCode(StatusCodes.Status404NotFound);
		}
	}
}
