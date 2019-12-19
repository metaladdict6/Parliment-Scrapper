using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Parliment_Scrapper.Services
{
    public class ScrappingService
    {
        public async Task<IHtmlDocument> ScrapUrlAsync(string url)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(url);
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);
            return document;
        }

        public void GetScrapeResults(IHtmlDocument document, string[] QueryTerms)
        {
            IEnumerable<IElement> articleLink = null;
            Dictionary<string, int> partySpoke = new Dictionary<string, int>();
            foreach (var element in document.All)
            {
                var containsLink = element.InnerHtml.Contains("<li>");
                var containsSubMenu = element.InnerHtml.Contains("submenu__link");
                var containsAHref = element.InnerHtml.Contains("<a href");
                if (!(containsLink || containsSubMenu || containsAHref))
                {
                    if (element.InnerHtml.Contains("<strong>"))
                    {
                        var text = element.InnerHtml;
                        var party = "";
                        int startIndexOfSubstance = 0;
                        if (text.Contains("</strong> ("))
                        {
                            var startIndex = text.IndexOf('(');
                            var endIndex = text.IndexOf(')');
                            startIndexOfSubstance = endIndex;
                            party = text.Substring(startIndex + 1, endIndex - startIndex - 1);
                        }

                        var speaker = FindSpeaker(text);

                        var speakerTitle = FindSpeakerTitle(text);
                        if (party != "")
                        {
                            text = RemoveWordFromString("(" + party + ")", text);
                        }
                        text = RemoveWordFromString("<strong>", text);
                        text = RemoveWordFromString("</strong>", text);
                        text = RemoveWordFromString("<br>", text);

                        Console.WriteLine("Speaker title: " + speakerTitle);
                        Console.WriteLine("Speaker: " + speaker);
                        Console.WriteLine("Partij: " + party);
                        startIndexOfSubstance = text.IndexOf("<br>");
                        Console.WriteLine("Tekst: " + text.Substring(startIndexOfSubstance + 4));
                    }
                }

            }
        }

        private string RemoveWordFromString(string toRemove, string toReturn)
        {

            return toReturn;
        }


        private string FindSpeakerTitle(string text)
        {
            int startIndex = text.IndexOf("<strong>");
            return text.Substring(0, startIndex);
        }

        private string FindSpeaker(string text)
        {
            int startIndex = text.IndexOf("<strong>");
            startIndex = startIndex + 8;
            int endIndex = text.IndexOf("</strong>");
            var speaker = text.Substring(startIndex, endIndex - startIndex);
            return speaker;
        }
    }
}
