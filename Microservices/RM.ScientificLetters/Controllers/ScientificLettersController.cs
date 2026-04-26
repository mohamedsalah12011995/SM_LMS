


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.ScientificLetters.Services;
using RM.ScientificLetters.Dtos;
using RM.ScientificLetters.Records;
using Mapster;

namespace RM.ScientificLetters.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScientificLettersController 
    {
        private readonly IScientificLettersService _scientificLettersService;
        public ScientificLettersController(IScientificLettersService scientificLettersService)
        {
            _scientificLettersService = scientificLettersService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetScientificLettersLookups()
        {
            return await _scientificLettersService.GetScientificLettersLookups();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetScientificLettersList(GetScientificLettersListRecord RequestedData)
        {
            return  await _scientificLettersService.GetScientificLettersList(RequestedData.Adapt<Dtos.ScientificLetters>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetScientificLettersDetails(GetScientificLettersDetailsRecord RequestedData)
        {
            return await _scientificLettersService.GetScientificLettersDetails(RequestedData.Adapt<Dtos.ScientificLetters>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveScientificLetters(SaveScientificLettersRecord RequestedData)
        {
            return  await _scientificLettersService.SaveScientificLetters(RequestedData.Adapt<Dtos.ScientificLetters>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> ModelActions(ModelActionsRecord RequestedData)
        {
            return await _scientificLettersService.ModelActions(RequestedData.Adapt<Dtos.ScientificLetters>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _scientificLettersService.GetPathOfResource(fileName);
        }
    }
}
