using DotNet_Task.Dtos;
using DotNet_Task.Models;

namespace DotNet_Task.Services.Interface
{
    public interface ICosmosDbService
    {
        Task<Question> AddQuestionAsync(QuestionDto questionDto);
        Task<Question> UpdateQuestionAsync(string id, QuestionDto questionDto);
        Task<Question> GetQuestionAsync(string id);
        Task<Candidate> AddCandidateAsync(CandidateDto candidateDto);
        Task<Candidate> GetCandidateAsync(string id);
    }
}
