using DotNet_Task.Dtos;
using DotNet_Task.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public QuestionsController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDto questionDto)
        {
            var question = await _cosmosDbService.AddQuestionAsync(questionDto);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody] QuestionDto questionDto)
        {
            var question = await _cosmosDbService.UpdateQuestionAsync(id, questionDto);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(string id)
        {
            var question = await _cosmosDbService.GetQuestionAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost("candidates")]
        public async Task<IActionResult> AddCandidate([FromBody] CandidateDto candidateDto)
        {
            var candidate = await _cosmosDbService.AddCandidateAsync(candidateDto);
            return CreatedAtAction(nameof(GetCandidate), new { id = candidate.Id }, candidate);
        }

        [HttpGet("candidates/{id}")]
        public async Task<IActionResult> GetCandidate(string id)
        {
            var candidate = await _cosmosDbService.GetCandidateAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return Ok(candidate);
        }
    }
}
