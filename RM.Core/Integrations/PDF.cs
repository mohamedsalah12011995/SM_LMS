using Irony.Parsing;
using MailKit.Net.Smtp;
using MimeKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace RM.Core.Integrations
{
    public class PDF
    {
        public static async Task<byte[]> GeneratePdfFromUrlAsync(string URL, PdfOptions PDFOptions)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(URL, WaitUntilNavigation.Networkidle0);
                return await page.PdfDataAsync(PDFOptions);
            }
        }

        public static async Task<byte[]> GeneratePdfFromUrlAsync(string URL, string PDFServiceUrl, PdfOptions PDFOptions, string Token)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            var PDFInfo = new { url = URL , pdfOptions = PDFOptions };

            var RequestEncapsulation = JsonSerializer.Serialize(PDFInfo);
            request = new HttpRequestMessage(HttpMethod.Post, PDFServiceUrl);
            request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return JsonSerializer.Deserialize<byte[]>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
