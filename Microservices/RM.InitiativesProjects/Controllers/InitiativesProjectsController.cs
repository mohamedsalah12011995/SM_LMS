

using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.InitiativesProjects.Dtos;
using RM.InitiativesProjects.Records;
using RM.InitiativesProjects.Services;

namespace RM.InitiativesProjects.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InitiativesProjectsController
    {
        private readonly IInitiativesProjectsService _initiativesProjectsHandler;
        public InitiativesProjectsController(IInitiativesProjectsService initiativesProjectsHandler)
        {
            _initiativesProjectsHandler = initiativesProjectsHandler;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookups()
        {
            return await _initiativesProjectsHandler.GetLookups();
        }

        [HttpPost]
        [Produces("application/json")]
        public async  Task<OperationOutput> GetInitiativesProjectList(GetInitiativesProjectListRecord RequestedData)
        {
            return await _initiativesProjectsHandler.GetInitiativesProjectList(RequestedData.Adapt<Dtos.InitiativesProjects>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetInitiativesProjectDetails(GetInitiativesProjectDetailsRecord RequestedData)
        {
            return await _initiativesProjectsHandler.GetInitiativesProjectDetails(RequestedData.Adapt<Dtos.InitiativesProjects>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> SaveInitiativesProject(SaveInitiativesProjectRecord RequestedData)
        {
            return  await _initiativesProjectsHandler.SaveInitiativesProject(RequestedData.Adapt<Dtos.InitiativesProjects>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
        {
            return await _initiativesProjectsHandler.ModelActions(RequestedData.Adapt<Dtos.InitiativesProjects>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
            return await _initiativesProjectsHandler.ModelActions(RequestedData.Adapt<Dtos.InitiativesProjects>());
        }
       
    }
}
