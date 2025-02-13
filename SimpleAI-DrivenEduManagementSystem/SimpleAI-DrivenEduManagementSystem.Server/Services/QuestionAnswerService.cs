using Microsoft.ML;
using Microsoft.ML.Data;
using MongoDB.Driver;
using SimpleAI_DrivenEduManagementSystem.Server.Models;
using SimpleAI_DrivenEduManagementSystem.Server.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

public class QuestionAnswerService : IQuestionAnswerService
{
    private readonly IMongoCollection<QAPair> _qaCollection;
    private readonly MLContext _mlContext;
    private readonly PredictionEngine<QAPair, QAResult> _predictionEngine;

    public QuestionAnswerService(IMongoDatabase database)
    {
        _qaCollection = database.GetCollection<QAPair>("QuestionsAnswers");
        _mlContext = new MLContext();

        var trainingData = LoadTrainingData();
        var pipeline = BuildPipeline();
        var model = pipeline.Fit(trainingData);
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<QAPair, QAResult>(model);
    }

    private IDataView LoadTrainingData()
    {
        var questions = _qaCollection.Find(_ => true).ToList();
        return _mlContext.Data.LoadFromEnumerable(questions);
    }

    private IEstimator<ITransformer> BuildPipeline()
    {
        return _mlContext.Transforms.Text.FeaturizeText("Features", nameof(QAPair.Question))
            .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Answer", featureColumnName: "Features"));
    }

    public string GetAnswer(string question)
    {
        var prediction = _predictionEngine.Predict(new QAPair { Question = question });
        return prediction.Answer ?? "I don't know the answer to that.";
    }
}
