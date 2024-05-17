using DotNet_Task.Dtos;
using DotNet_Task.Models;
using DotNet_Task.Services.Interface;
using Microsoft.Azure.Cosmos;

public class CosmosDbService : ICosmosDbService
{
    private Container _questionsContainer;
    private Container _candidatesContainer;

    public CosmosDbService(CosmosClient dbClient, string databaseName, string containerNameQuestions, string containerNameCandidates)
    {
        this._questionsContainer = dbClient.GetContainer(databaseName, containerNameQuestions);
        this._candidatesContainer = dbClient.GetContainer(databaseName, containerNameCandidates);
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

        await this._questionsContainer.CreateItemAsync<Question>(question, new PartitionKey(question.Id));
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

        await this._questionsContainer.UpsertItemAsync<Question>(question, new PartitionKey(id));
        return question;
    }

    public async Task<Question> GetQuestionAsync(string id)
    {
        try
        {
            ItemResponse<Question> response = await this._questionsContainer.ReadItemAsync<Question>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
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

        await this._candidatesContainer.CreateItemAsync<Candidate>(candidate, new PartitionKey(candidate.Id));
        return candidate;
    }

    public async Task<Candidate> GetCandidateAsync(string id)
    {
        try
        {
            ItemResponse<Candidate> response = await this._candidatesContainer.ReadItemAsync<Candidate>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}
