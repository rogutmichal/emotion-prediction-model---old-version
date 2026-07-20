namespace EmotionAnalyzerML.Models
{
    public class PredictionRequest
    {
        // Text to be analyzed for emotion prediction
        public string Text { get; set; } = string.Empty;
    }
}