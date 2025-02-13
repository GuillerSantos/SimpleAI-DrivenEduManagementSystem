using Microsoft.Extensions.ML;
using SimpleAI_DrivenEduManagementSystem.Server.Models;
using SimpleAI_DrivenEduManagementSystem.Server.Services;
using SimpleAI_DrivenEduManagementSystem.Server.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));

// Register MongoDB service
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<IQuestionAnswerService, QuestionAnswerService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
