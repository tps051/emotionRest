using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleEmotionsRESTAPI
{
    class Program
    {
        private const string subkey = "6079a1e2a57b4b9a84d904659cc1d40c";
        private const string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";

        private static List<string> imagePaths = new List<string>
        {
            @"C:\Photo\1.jpg",
            @"C:\Photo\2.jpg",
            @"C:\Photo\3.jpg",
            @"C:\Photo\4.jpg",
            @"C:\Photo\5.jpg",
            @"C:\Photo\6.jpg"
        };

        private static string result = "";
        private static int photo = 0;

        static void Main(string[] args)
        {
            Console.WriteLine($"There are {imagePaths.Count} photo's links in list. Please, choose one: ");
            photo = Int32.Parse(Console.ReadLine()) - 1;
            GetEmotions();

            Console.ReadKey();
        }

        static byte[] GetImage(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(file);
            return bin.ReadBytes((int) file.Length);
        }

        static async void GetEmotions()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subkey);
            byte[] data = GetImage(imagePaths[photo]);
            HttpResponseMessage response;

            using (var content = new ByteArrayContent(data))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                try
                {
                    response = await client.PostAsync(uri, content);
                    result = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
               
            }
            List<FaceObject> lst = new List<FaceObject>();
            try
            {
                lst = JsonConvert.DeserializeObject<List<FaceObject>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            foreach (var item in lst)
            {
                Console.WriteLine($"Face:                      \t{lst.IndexOf(item) + 1}");
                Console.WriteLine($"Anger:                     \t{item.scores.anger}");
                Console.WriteLine($"Contempt:                  \t{item.scores.contempt}");
                Console.WriteLine($"Disgust:                   \t{item.scores.disgust}");
                Console.WriteLine($"Fear:                      \t{item.scores.fear}");
                Console.WriteLine($"Happiness:                 \t{item.scores.happiness}");
                Console.WriteLine($"Neutral:                   \t{item.scores.neutral}");
                Console.WriteLine($"Sadness:                   \t{item.scores.sadness}");
                Console.WriteLine($"Surprise:                  \t{item.scores.surprise}");
                Console.WriteLine($"The most bright emotion is:\t{MaxEmotion(item.scores)}");
                Console.WriteLine("_____________________________________________________");
            }
        }

        static string MaxEmotion(Scores score)
        {
            string result = "";
            double max = 0;
            if (score.anger > max)
            {
                result = "anger";
                max = score.anger;
            }
            if (score.contempt > max)
            {
                result = "contempt";
                max = score.contempt;
            }
            if (score.disgust > max)
            {
                result = "disgust";
                max = score.disgust;
            }
            if (score.fear > max)
            {
                result = "fear";
                max = score.fear;
            }
            if (score.happiness > max)
            {
                result = "happines";
                max = score.happiness;
            }
            if (score.neutral > max)
            {
                result = "neutral";
                max = score.neutral;
            }
            if (score.sadness > max)
            {
                result = "sadness";
                max = score.sadness;
            }
            if (score.surprise > max)
            {
                result = "surprise";
                max = score.surprise;
            }
            return result;
        }
    }
}
