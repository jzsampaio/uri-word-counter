using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using HtmlAgilityPack;

namespace StudyHoursEstimate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            using(var file = File.OpenText("urls.csv"))
            {
                while(!file.EndOfStream)
                {
                    var uri = new Uri(file.ReadLine());
                    WriteLine(uri);
                    var rawHtml = await httpClient.GetStringAsync(uri);
                    var html = new HtmlDocument();
                    html.LoadHtml(rawHtml);
                    var textNodes = html.DocumentNode
                        .SelectSingleNode(@"//*[@id=""main-column""]")
                        .Descendants()
                        .Where(n => !n.HasChildNodes && !String.IsNullOrWhiteSpace(n.InnerText));

                    var fullText = String.Join("\n", textNodes.Select(x =>x.InnerText));
                    var wordCount = textNodes.Select(x => x.InnerText.Split(" ").Count()).Sum();

                    WriteLine(fullText);
                    WriteLine($"Word count: {wordCount}");
                }
            }
        }
    }
}
