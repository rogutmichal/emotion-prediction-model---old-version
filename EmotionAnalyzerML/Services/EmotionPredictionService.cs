using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace EmotionAnalyzerML.Services
{
    public class EmotionPredictionService
    {
        private readonly MLContext _context;
        private readonly ITransformer _model;


        private static readonly string[] EmotionLabels =
        {
            "sadness",
            "anger",
            "love",
            "surprise",
            "fear",
            "joy"
        };


        public EmotionPredictionService(
            MLContext context,
            ITransformer model)
        {
            _context = context;
            _model = model;
        }


        public Dictionary<string, float> Predict(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Text cannot be empty");
            }


            var input = new TextData
            {
                Text = text
            };


            var data = new List<TextData>
            {
                input
            };


            var dataView = _context
                .Data
                .LoadFromEnumerable(data);


            var transformedData =
                _model.Transform(dataView);


            var scores =
                transformedData
                .GetColumn<float[]>("Score")
                .First();


            return EmotionLabels
                .Select((label, index) => new
                {
                    label,
                    score = scores[index]
                })
                .OrderByDescending(x => x.score)
                .Take(3)
                .ToDictionary(
                    x => x.label,
                    x => x.score);
        }
    }
}