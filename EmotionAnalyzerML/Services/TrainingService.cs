using EmotionAnalyzerML.Models;
using Microsoft.ML;


namespace EmotionAnalyzerML.Services
{
    public class TrainingService
    {
        private readonly MLContext _context;
        private readonly EmotionModelTrainer _trainer;



        public TrainingService(
            MLContext context,
            EmotionModelTrainer trainer)
        {
            _context = context;
            _trainer = trainer;
        }



        public void TrainAndSave(
            List<TextData> trainingData,
            string modelPath)
        {

            Console.WriteLine(
                "Training model...");



            var model =
                _trainer.TrainModel(
                    trainingData);



            var dataView =
                _context.Data
                .LoadFromEnumerable(
                    trainingData);



            var directory =
                Path.GetDirectoryName(
                    modelPath);



            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(
                    directory);
            }



            _context.Model.Save(
                model,
                dataView.Schema,
                modelPath);



            Console.WriteLine(
                "Model saved.");
        }
    }
}