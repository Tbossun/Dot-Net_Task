using DotNet_Task.Data;
using DotNet_Task.Dtos;
using DotNet_Task.Models;
using DotNet_Task.Services.Interface;

namespace DotNet_Task.Services
{
    public class SqliteDbService : ICosmosDbService
    {
        private readonly ApplicationDbContext _context;

        public SqliteDbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Question> AddQuestionAsync(QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Type = questionDto.Type,
                Text = questionDto.Text,
                Options = questionDto.Options
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateQuestionAsync(string id, QuestionDto questionDto)
        {
            var question = await GetQuestionAsync(id);
            if (question == null)
            {
                return null;
            }

            question.Type = questionDto.Type;
            question.Text = questionDto.Text;
            question.Options = questionDto.Options;

            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<Question> GetQuestionAsync(string id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task<Candidate> AddCandidateAsync(CandidateDto candidateDto)
        {
            var candidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = candidateDto.FirstName,
                LastName = candidateDto.LastName,
                Email = candidateDto.Email,
                Phone = candidateDto.Phone,
                Nationality = candidateDto.Nationality,
                CurrentResidence = candidateDto.CurrentResidence,
                IdNumber = candidateDto.IdNumber,
                Dob = candidateDto.Dob,
                Gender = candidateDto.Gender,
                Answers = candidateDto.Answers.Select(a => new Answer { QuestionId = a.QuestionId, Response = a.Response }).ToList()
            };

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate> GetCandidateAsync(string id)
        {
            return await _context.Candidates.FindAsync(id);
        }
    }
}
