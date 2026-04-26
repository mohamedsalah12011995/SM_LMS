
using PuppeteerSharp;

namespace PDFService.Records
{
    public record PDFInfoRecord
    {
        public string url { get; set; }
        public PdfOptions pdfOptions { get; set; }
    }
}
