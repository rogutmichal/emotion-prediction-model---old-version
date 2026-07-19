using EmotionAnalyzerML;
using EmotionAnalyzerML.Data;
using Xunit;

namespace EmotionAnalyzer.Tests;

public class DataLoaderTests
{
    // Test to ensure that the LoadDataFromFile method returns a non-null and non-empty list of TextData
    [Fact]
    public void LoadData_ShouldReturnItems()
    {
        // Arrange
        var path = "TestData/test.txt";

        // Act
        var result = DataLoader.LoadDataFromFile(path);


        // Assert
        Assert.NotNull(result);

        Assert.NotEmpty(result);
    }
    // Test to ensure that the LoadDataFromFile method reads the correct text from the file
    [Fact]
    public void LoadData_ShouldReadCorrectText()
    {
        // Arrange
        var result = DataLoader.LoadDataFromFile("TestData/test.txt");

        // Assert
        Assert.Equal("I love pizza", result.First().Text);
    }

    // Test to ensure that the LoadDataFromFile method reads the correct emotion from the file
    [Fact]
    public void LoadData_ShouldReadCorrectEmotion()
    {
        // Arrange
        var result = DataLoader.LoadDataFromFile("TestData/test.txt");

        // Assert
        Assert.Equal("joy", result.First().Emotion);
    }
}