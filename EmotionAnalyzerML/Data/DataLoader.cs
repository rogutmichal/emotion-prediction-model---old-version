using EmotionAnalyzerML.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionAnalyzerML.Data
{

    //Class is responsible for loading data from a CSV file and returning a list of TextData objects.
    public class DataLoader
    {
        // Load data from a CSV file and return a list of TextData objects
        public static List<TextData> LoadDataFromFile(string filePath)
        {
            
            var reviews = new List<TextData>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Split the line by semicolon to separate the text and emotion
                var parts = line.Split(';');
                if (parts.Length == 2)
                {
                    // Create a new TextData object and add it to the list
                    var review = new TextData
                    {
                        Text = parts[0].Trim(),
                        Emotion = parts[1].Trim()
                    };

                    reviews.Add(review);
                }
            }
            return reviews;
        }
    }
}