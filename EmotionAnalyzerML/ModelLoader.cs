using Microsoft.ML;


namespace EmotionAnalyzerML.Services
{
    public class ModelLoader
    {
        private readonly MLContext _context;


        public ModelLoader(
            MLContext context)
        {
            _context = context;
        }



        public ITransformer Load(
            string modelPath)
        {
            if (string.IsNullOrWhiteSpace(modelPath))
            {
                throw new ArgumentException(
                    "Model path cannot be empty.");
            }



            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException(
                    "Model file was not found.",
                    modelPath);
            }



            return _context.Model.Load(
                modelPath,
                out _);
        }
    }
}