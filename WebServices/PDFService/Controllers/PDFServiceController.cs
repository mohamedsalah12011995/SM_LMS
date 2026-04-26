using Microsoft.AspNetCore.Mvc;
using PDFService.Records;
using PDFService.Services;

namespace PDFService.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PDFServiceController
    {
        private readonly IPDFServices _PDFService;

        public PDFServiceController(IPDFServices PDFService)
        {
            _PDFService = PDFService;
        }

        [HttpPost]
        public async Task<byte[]> GeneratePdfFromUrlAsync(PDFInfoRecord RequestedData)
        {
            return await _PDFService.GeneratePdfFromUrlAsync(RequestedData);
        }
    }

}
