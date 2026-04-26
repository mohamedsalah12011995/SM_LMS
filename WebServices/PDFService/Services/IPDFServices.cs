using PDFService.Records;

namespace PDFService.Services
{
    public interface IPDFServices
    {
        Task<byte[]> GeneratePdfFromUrlAsync(PDFInfoRecord PDFInfo);
    }
}
