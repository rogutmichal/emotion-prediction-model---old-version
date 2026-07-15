using EmotionAnalyzerML;
using EmotionAnalyzerML.Models;
using EmotionAnalyzerML.Services;
using EmotionAnalyzerML.Training;
using Microsoft.ML;


public class EmotionBasedRecommendation
{
    private static readonly string ModelPath = "emotion_model.zip";
    private static readonly string TrainFilePath = "Data/train.txt";
    private static readonly string ValFilePath = "Data/val.txt";
    private static readonly string TestFilePath = "Data/test.txt";


    public static void Main(string[] args)
    {
        var context = new MLContext();


        ITransformer model;


        // Jeżeli model nie istnieje - trenujemy i zapisujemy
        if (!File.Exists(ModelPath))
        {
            Console.WriteLine("Model nie istnieje. Rozpoczynam trening...");


            var trainData = DataLoader.LoadDataFromFile(
                TrainFilePath);


            var trainer = new TrainingService(
                context);


            trainer.TrainAndSave(
                trainData,
                ModelPath);
        }


        // Pobranie gotowego modelu
        Console.WriteLine("Ładowanie modelu...");

        model = ModelLoader.Load(
            context,
            ModelPath);



        // Serwis predykcji
        var emotionService = new EmotionPredictionService(
            context,
            model);



        Console.WriteLine();
        Console.WriteLine("Enter text for analysis:");

        var userReview = Console.ReadLine();



        var emotions = emotionService.Predict(
            userReview);



        Console.WriteLine();
        Console.WriteLine($"Text: {userReview}");

        Console.WriteLine("Emotions:");

        foreach (var emotion in emotions)
        {
            Console.WriteLine(
                $"{emotion.Key}: {emotion.Value:P2}");
        }



        Console.ReadKey();
    }

}