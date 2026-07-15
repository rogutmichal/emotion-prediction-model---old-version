using Microsoft.ML;

namespace EmotionAnalyzerML
{
    public class ModelLoader
    {
        public static ITransformer Load(
            MLContext context,
            string modelPath)
        {
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException(
                    "Model file was not found",
                    modelPath);
            }


            return context.Model.Load(
                modelPath,
                out _);
        }
    }
}