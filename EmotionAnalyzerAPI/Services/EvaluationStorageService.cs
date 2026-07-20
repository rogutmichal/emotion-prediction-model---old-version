using System.Text.Json;
using EmotionAnalyzerML.Models;

namespace EmotionAnalyzerAPI.Services
{
    // Service for saving and loading model evaluation results
    public class EvaluationStorageService
    {
        
        private readonly string path = "Data/evaluation.json";
        // Save the evaluation result to a JSON file
        public void Save(ModelEvaluationResult result)
        {
            var json =
                JsonSerializer.Serialize(
                    result,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(path,json);
        }


        // Load the evaluation result from a JSON file
        public ModelEvaluationResult? Load()
        {
            if (!File.Exists(path))
            {
                return null;
            }


            var json = File.ReadAllText(path);


            return JsonSerializer.Deserialize<ModelEvaluationResult>(json);
        }
    }
}