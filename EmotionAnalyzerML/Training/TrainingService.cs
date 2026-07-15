using Microsoft.ML;
using EmotionAnalyzerML.Models;

namespace EmotionAnalyzerML.Training
{
    public class TrainingService
    {
        private readonly MLContext _context;


        public TrainingService(
            MLContext context)
        {
            _context = context;
        }


        public void TrainAndSave(
            List<TextData> trainingData,
            string modelPath)
        {
            Console.WriteLine(
                "Training model...");


            var model =
                EmotionModelTrainer.TrainModel(
                    _context,
                    trainingData);


            var dataView =
                _context.Data.LoadFromEnumerable(
                    trainingData);


            _context.Model.Save(
                model,
                dataView.Schema,
                modelPath);


            Console.WriteLine(
                "Model saved.");
        }
    }
}