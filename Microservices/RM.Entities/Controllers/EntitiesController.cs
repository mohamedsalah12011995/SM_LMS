using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RM.Entities.Services;
using RM.Entities.Dtos;
using RM.Entities.Records;
using Mapster;

namespace RM.Entities.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EntitiesController 
    {
        private readonly IEntitiesService _entitiesService;
        public EntitiesController(IEntitiesService entitiesService)
        {
            _entitiesService = entitiesService;
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetEntityLookups()
        {
            return _entitiesService.GetEntityLookups();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveEntity(SaveEntityRecord RequestedData)
        {
            return await _entitiesService.SaveEntity(RequestedData.Adapt<Dtos.Entity>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetEntityDetails(GetEntityDetailsRecord RequestedData)
        {
            return await _entitiesService.GetEntityDetails(RequestedData.Adapt<Dtos.Entity>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetEntitiesList(GetEntitiesListRecord RequestedData)
        {
           return await _entitiesService.GetEntitiesList(RequestedData.Adapt<Dtos.Entity>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput EntityActivation(EntityActivationRecord RequestedData)
        {
           return _entitiesService.EntityActivation(RequestedData.Adapt<Dtos.Entity>());
        }


    }
}
