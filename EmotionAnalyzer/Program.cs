using EmotionAnalyzer.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EmotionAnalyzer;






public class EmotionBasedRecommendation
{
    private static readonly string ModelPath = "emotion_model.zip";
    private static readonly string TrainFilePath = "Data/train.txt";
    private static readonly string ValFilePath = "Data/val.txt";
    private static readonly string TestFilePath = "Data/test.txt";
    public static void Main(string[] args)
    {
        var context = new MLContext();
        var trainTexts = DataLoader.LoadDataFromFile(TrainFilePath);
        ITransformer model;

        if (!File.Exists(ModelPath))
        {
            Console.WriteLine("Trenowanie modelu");
            model = EmotionModelTrainer.TrainModel(context, trainTexts);
            context.Model.Save(model, context.Data.LoadFromEnumerable(trainTexts).Schema, ModelPath);
        }
        else
        {
            Console.WriteLine("Ładowanie modelu");
            model = context.Model.Load(ModelPath, out _);
        }
        Console.WriteLine("Wpisz recenzję:");
        string review = Console.ReadLine();

        var exampleText = new TextData
        {
            Text = review
        };

        var top3Emotions = EmotionModel.PredictEmotion(context, model, exampleText);

        Console.WriteLine("\nWynik analizy emocji:");
        foreach (var kvp in top3Emotions)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value:P2}");
        }

        var valReviews = DataLoader.LoadDataFromFile(ValFilePath);
        ModelEvaluator.TestModel(context, model, valReviews, "VALIDATION");

        var testReviews = DataLoader.LoadDataFromFile(TestFilePath);
        ModelEvaluator.TestModel(context, model, testReviews, "TEST");

        Console.ReadKey();
    }
}