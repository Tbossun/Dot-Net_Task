using DotNet_Task.Services.Interface;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure Cosmos DB client
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb");
    return new CosmosClient(connectionString);
});

// Register the CosmosDbService
builder.Services.AddScoped<ICosmosDbService>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
    var containerNameQuestions = builder.Configuration["CosmosDb:ContainerNameQuestions"];
    var containerNameCandidates = builder.Configuration["CosmosDb:ContainerNameCandidates"];
    return new CosmosDbService(cosmosClient, databaseName, containerNameQuestions, containerNameCandidates);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
