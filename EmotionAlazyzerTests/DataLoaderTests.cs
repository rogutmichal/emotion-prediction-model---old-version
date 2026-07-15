using EmotionAnalyzerML;
using EmotionAnalyzerML.Data;
using Xunit;

namespace EmotionAnalyzer.Tests;

public class DataLoaderTests
{
    [Fact]
    public void LoadData_ShouldReturnItems()
    {

        var path = "TestData/test.txt";


        var result = DataLoader.LoadDataFromFile(path);


        Assert.NotNull(result);

        Assert.NotEmpty(result);
    }

    [Fact]
    public void LoadData_ShouldReadCorrectText()
    {
        var result =
            DataLoader.LoadDataFromFile(
                "TestData/test.txt");

        Assert.Equal(
            "I love pizza",
            result.First().Text);
    }


    [Fact]
    public void LoadData_ShouldReadCorrectEmotion()
    {
        var result =
            DataLoader.LoadDataFromFile(
                "TestData/test.txt");

        Assert.Equal(
            "joy",
            result.First().Emotion);
    }
}