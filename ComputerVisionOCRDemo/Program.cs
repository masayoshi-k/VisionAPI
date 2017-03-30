using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;

namespace ComputerVisionOCRDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("分析するファイルパスを指定してください: ");
            string imageFilePath = Console.ReadLine();

            Uri fileUri = new Uri(imageFilePath);
            var result = DoWork(fileUri, true);
            Console.ReadKey();
        }

        static async Task<OcrResults> UploadAndRecognizeImage(string imageFilePath, string langage)
        {
            // Create Project Oxford Vision API Service client
            VisionServiceClient VisionServiceClient = new VisionServiceClient("3a2903d88d3a4f4e9f76531a5fc77cb0");

            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                // Upload an image and perform OCR
                OcrResults ocrResult = await VisionServiceClient.RecognizeTextAsync(imageFileStream, langage);
                return ocrResult;
            }
        }

        // Perform the work for this scenario
        static async Task DoWork(Uri imageUri,bool upload)
        {
            Console.WriteLine("Executing OCR...");

            //string languageCode = "ja";
            string languageCode = "en";
            var ocrResult = new OcrResults();
            ocrResult = await UploadAndRecognizeImage(imageUri.LocalPath, languageCode);
            Console.WriteLine("OCR Complete");

            // Log analysis result in the log window
            Console.WriteLine("");
            Console.WriteLine("Results are shown below:");
            LogOcrResults(ocrResult);
        }

        static void LogOcrResults(OcrResults results)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (results != null && results.Regions != null)
            {
                stringBuilder.Append("Details:");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }
                    stringBuilder.AppendLine();
                }
            }
            Console.WriteLine(stringBuilder.ToString());
        }

    }
}
