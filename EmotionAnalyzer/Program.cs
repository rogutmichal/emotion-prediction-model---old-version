using EmotionAnalyzerML;
using EmotionAnalyzerML.Services;
using Microsoft.ML;
using Microsoft.Extensions.Configuration;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Data;


public class EmotionBasedRecommendation
{
    public static void Main(string[] args)
    {

        var configuration =
            new ConfigurationBuilder()
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .Build();



        var modelSettings =
            configuration
                .GetSection("ModelSettings")
                .Get<ModelSettings>();



        var context =
            new MLContext();




        // Model loading service

        var modelLoader =
            new ModelLoader(
                context);



        // Model stored in memory

        var loadedModel =
            new LoadedModelService();




        // Load model once

        if (File.Exists(modelSettings.ModelPath))
        {
            loadedModel.LoadModel(
                modelLoader,
                modelSettings.ModelPath);
        }




        // Training services

        var modelTrainer =
            new EmotionModelTrainer(
                context);



        var trainingService =
            new TrainingService(
                context,
                modelTrainer);






        while (true)
        {
            Console.Clear();

            Console.WriteLine("==============================");
            Console.WriteLine("     Emotion Analyzer");
            Console.WriteLine("==============================");
            Console.WriteLine();

            Console.WriteLine("1. Text analysis");
            Console.WriteLine("2. Evaluation");
            Console.WriteLine("3. Model training");
            Console.WriteLine("0. Exit");

            Console.WriteLine();

            Console.Write("Select options: ");


            var choice =
                Console.ReadLine();




            switch (choice)
            {
                case "1":

                    PredictEmotion(
                        context,
                        loadedModel);

                    break;



                case "2":

                    EvaluateModel(
                        context,
                        loadedModel,
                        modelSettings);

                    break;



                case "3":

                    TrainModel(
                        modelSettings,
                        trainingService,
                        loadedModel,
                        modelLoader);

                    break;



                case "0":

                    return;



                default:

                    Console.WriteLine(
                        "Invalid option.");

                    break;
            }




            Console.WriteLine();

            Console.WriteLine(
                "Press any key to continue...");

            Console.ReadKey();

        }
    }







    private static void PredictEmotion(
        MLContext context,
        LoadedModelService loadedModel)
    {
        Console.WriteLine();

        Console.WriteLine(
            "=== Text analysis ===");




        if (!loadedModel.IsLoaded)
        {
            Console.WriteLine(
                "Model not loaded. Train model first.");

            return;
        }




        var emotionService =
    new EmotionPredictionService(
        context,
        loadedModel);




        Console.Write(
            "Enter text: ");



        var text =
            Console.ReadLine();





        var result =
            emotionService.Predict(
                text);





        Console.WriteLine();

        Console.WriteLine(
            $"Text: {result.Text}");



        Console.WriteLine();

        Console.WriteLine(
            "Result:");





        foreach (var prediction in result.Predictions)
        {
            Console.WriteLine(
                $"{prediction.Emotion}: {prediction.Confidence:P2}");
        }

    }


    private static void EvaluateModel(
        MLContext context,
        LoadedModelService loadedModel,
        ModelSettings modelSettings)
    {
        Console.WriteLine();

        Console.WriteLine(
            "=== Model evaluation ===");




        if (!loadedModel.IsLoaded)
        {
            Console.WriteLine(
                "Model not loaded.");

            return;
        }





        var testData =
            DataLoader.LoadDataFromFile(
                modelSettings.TestFilePath);





        var evaluator =
            new ModelEvaluationService(
                context);





        var result =
            evaluator.Evaluate(
                loadedModel.Model,
                testData,
                "TEST");





        Console.WriteLine();

        Console.WriteLine(
            $"Micro Accuracy: {result.MicroAccuracy:P2}");

        Console.WriteLine(
            $"Macro Accuracy: {result.MacroAccuracy:P2}");

        Console.WriteLine(
            $"LogLoss: {result.LogLoss:F4}");

    }










    private static void TrainModel(
        ModelSettings modelSettings,
        TrainingService trainingService,
        LoadedModelService loadedModel,
        ModelLoader modelLoader)
    {
        Console.WriteLine();

        Console.WriteLine(
            "=== Model training ===");





        var trainData =
            DataLoader.LoadDataFromFile(
                modelSettings.TrainFilePath);





        trainingService.TrainAndSave(
            trainData,
            modelSettings.ModelPath);





        // Po treningu przeładuj model do pamięci

        loadedModel.LoadModel(
            modelLoader,
            modelSettings.ModelPath);





        Console.WriteLine();

        Console.WriteLine(
            "The model has been saved and loaded.");

    }
}