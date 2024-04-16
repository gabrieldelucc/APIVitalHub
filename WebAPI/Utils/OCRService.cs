using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    public class OCRService
    {
        private readonly string _subscriptionKey = "63a5b309fd4c4e0aae338f12aa43a7e8";
        private readonly string _endpoint = "https://cvvitalhubgabrielg2t.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
        {
            try
            {
                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey))
                {
                    Endpoint = _endpoint
                };

                // Chamar RecognizePrintedTextInStreamAsync com o Stream fornecido
                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                // Processar o resultado de reconhecimento
                return ProcessRecognitionResult(ocrResult);
            }
            catch (Exception ex)
            {
                return "Erro ao reconhecer o texto: " + ex.Message;
            }
        }

        private static string ProcessRecognitionResult(OcrResult result)
        {
            try
            {
                string recognizedText = "";

                foreach (var region in result.Regions)
                {
                    foreach (var line in region.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            recognizedText += " " + word.Text;
                        }
                        recognizedText += "\n";
                    }
                }

                return recognizedText.Trim(); // Retorna o texto reconhecido (removendo espaços extras)
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
