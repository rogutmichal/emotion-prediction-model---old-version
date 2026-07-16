using EmotionAnalyzerML.Models;
using Microsoft.ML;
using Microsoft.ML.Trainers.LightGbm;


namespace EmotionAnalyzerML.Services
{
    public class EmotionModelTrainer
    {
        private readonly MLContext _context;


        public EmotionModelTrainer(
            MLContext context)
        {
            _context = context;
        }



        public ITransformer TrainModel(
            List<TextData> texts)
        {

            if (texts == null || texts.Count == 0)
            {
                throw new ArgumentException(
                    "Training data cannot be empty.");
            }



            // Obliczenie wag klas
            var classCounts =
                texts
                .GroupBy(x => x.Emotion)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count());



            int maxCount =
                classCounts.Values.Max();



            var weightedTexts =
                texts
                .Select(x => new WeightedData
                {
                    Text = x.Text,

                    Emotion = x.Emotion,

                    Weight =
                        (float)maxCount /
                        classCounts[x.Emotion]

                })
                .ToList();




            var trainData =
                _context.Data
                .LoadFromEnumerable(
                    weightedTexts);



            var cachedTrainData =
                _context.Data.Cache(
                    trainData);




            var pipeline =
                _context.Transforms
                .Conversion
                .MapValueToKey(
                    "Label",
                    "Emotion")



                .Append(
                    _context.Transforms.Text
                    .NormalizeText(
                        "NormalizedText",
                        "Text"))



                .Append(
                    _context.Transforms.Text
                    .TokenizeIntoWords(
                        "Tokens",
                        "NormalizedText"))



                .Append(
                    _context.Transforms.Text
                    .RemoveDefaultStopWords(
                        "TokensClean",
                        "Tokens"))



                .Append(
                    _context.Transforms.Conversion
                    .MapValueToKey(
                        "TokensKeys",
                        "TokensClean"))



                .Append(
                    _context.Transforms.Text
                    .ProduceNgrams(
                        "UniGrams",
                        "TokensKeys",
                        ngramLength: 1,
                        useAllLengths: false))



                .Append(
                    _context.Transforms.Text
                    .ProduceNgrams(
                        "BiGrams",
                        "TokensKeys",
                        ngramLength: 2,
                        useAllLengths: false))



                .Append(
                    _context.Transforms.Text
                    .ProduceNgrams(
                        "TriGrams",
                        "TokensKeys",
                        ngramLength: 3,
                        useAllLengths: false))



                .Append(
                    _context.Transforms
                    .Concatenate(
                        "Features",
                        "UniGrams",
                        "BiGrams",
                        "TriGrams"))



                .Append(
                    _context.MulticlassClassification
                    .Trainers
                    .LightGbm(
                        new LightGbmMulticlassTrainer.Options
                        {
                            LabelColumnName =
                                "Label",


                            FeatureColumnName =
                                "Features",


                            ExampleWeightColumnName =
                                "Weight",


                            NumberOfLeaves = 104,


                            MinimumExampleCountPerLeaf = 5,


                            LearningRate = 0.006,


                            NumberOfIterations = 875
                        }))



                .Append(
                    _context.Transforms
                    .Conversion
                    .MapKeyToValue(
                        "PredictedEmotion",
                        "PredictedLabel"));




            return pipeline.Fit(
                cachedTrainData);
        }
    }
}