using DotNet_Task.Data;
using DotNet_Task.Services;
using DotNet_Task.Services.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure Cosmos DB client
/*builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb");
    return new CosmosClient(connectionString);
});*/

// Configure SQLite DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

// Register the SQLite service
builder.Services.AddScoped<ICosmosDbService, SqliteDbService>();

// Register the CosmosDbService
/*builder.Services.AddScoped<ICosmosDbService>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
    var containerNameQuestions = builder.Configuration["CosmosDb:ContainerNameQuestions"];
    var containerNameCandidates = builder.Configuration["CosmosDb:ContainerNameCandidates"];
    return new CosmosDbService(cosmosClient, databaseName, containerNameQuestions, containerNameCandidates);
});*/

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
