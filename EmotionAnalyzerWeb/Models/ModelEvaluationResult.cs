namespace EmotionAnalyzerWeb.Models;

//Evaluation metrics for the model
public class ModelEvaluationResult
{
    public string DatasetName { get; set; } = "";

    //Basic metrics
    public double MicroAccuracy { get; set; }

    public double MacroAccuracy { get; set; }

    public double LogLoss { get; set; }

    //Confusion matrix

    public List<string> Labels { get; set; } = new();


    public long[][] ConfusionMatrix { get; set; } = Array.Empty<long[]>();


    //Per-class metrics
    public Dictionary<string, double> Precision { get; set; } = new();

    public Dictionary<string, double> Recall { get; set; } = new();

    public Dictionary<string, double> F1Score { get; set; } = new();
}