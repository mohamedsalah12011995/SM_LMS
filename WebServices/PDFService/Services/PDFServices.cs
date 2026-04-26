using RM.Core.CommonDtos;
using RM.Core.Services;
using PDFService.Records;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using Microsoft.Extensions.FileProviders;

namespace PDFService.Services
{
    public class PDFServices: WebBaseService, IPDFServices
    {
        public PDFServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, configuration)
        {

        }

        public async Task<byte[]> GeneratePdfFromUrlAsync(PDFInfoRecord PDFInfo)
        {
            if (RequestOwner == null)
                return null;

            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"Chrome/chrome.exe")))
            {
                var chromiumPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Chrome")).Root;
                await new BrowserFetcher().DownloadAsync();
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, ExecutablePath = chromiumPath + "/chrome.exe" }))

                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(PDFInfo.url, WaitUntilNavigation.Networkidle0);
                    return await page.PdfDataAsync(PDFInfo.pdfOptions);
                }
            }
            else
            {
                await new BrowserFetcher().DownloadAsync();
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(PDFInfo.url, WaitUntilNavigation.Networkidle0);
                    return await page.PdfDataAsync(PDFInfo.pdfOptions);
                }
            }
        }
    }
}
