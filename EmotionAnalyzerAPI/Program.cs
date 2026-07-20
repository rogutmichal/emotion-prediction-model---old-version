using EmotionAnalyzerAPI.Services;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using Microsoft.ML;


var builder = WebApplication.CreateBuilder(args);



builder.Configuration.Sources.Clear();


builder.Configuration
    .AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: false)
    .AddEnvironmentVariables();


//Controllers
builder.Services.AddControllers();

//Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configuration

builder.Services.Configure<ModelSettings>(builder.Configuration.GetSection("ModelSettings"));


// MLContext
builder.Services.AddSingleton<MLContext>();


//Model Services
builder.Services.AddSingleton<ModelLoader>();

builder.Services.AddSingleton<LoadedModelService>();

//Training Services

builder.Services.AddSingleton<EmotionModelTrainer>();

builder.Services.AddSingleton<TrainingService>();


// Model Initialization
builder.Services.AddSingleton<ModelInitializer>();

// Prediction Services

builder.Services.AddSingleton<EmotionPredictionService>();

// Evaluation Services

builder.Services.AddSingleton<EvaluationStorageService>();
builder.Services.AddSingleton<ModelEvaluationService>();



var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ModelInitializer>();

    initializer.Initialize();
}
// Root endpoint to check if the API is running

app.MapGet("/", () =>
{
    return "Emotion Analyzer API is running";
});


app.Run();