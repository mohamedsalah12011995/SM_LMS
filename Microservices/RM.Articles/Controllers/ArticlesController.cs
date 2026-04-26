using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Articles.Dtos;
using RM.Articles.Records;
using RM.Articles.Services;
using RM.Core.Helpers;

namespace RM.Articles.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController(IArticlesService articleService) 
    {
        private readonly IArticlesService _articleService = articleService;


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetArticleDetails(GetArticleDetailsRecord RequestedData)
        {
            return await _articleService.GetArticleDetails(RequestedData.Adapt<Dtos.Articles>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveArticle(SaveArticleRecord RequestedData)
        {
            return await _articleService.SaveArticle(RequestedData.Adapt<Dtos.Articles>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetArticlesList(GetArticlesListRecord RequestedData)
        {
            return await _articleService.GetArticlesList(RequestedData.Adapt<Dtos.Articles>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Activation(ActivationRecord RequestedData)
        {
            return _articleService.ModelActions(RequestedData.Adapt<Dtos.Articles>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Delete(DeleteRecord RequestedData)
        {
            return _articleService.ModelActions(RequestedData.Adapt<Dtos.Articles>());
        }
    }
}
