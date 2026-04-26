using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.News.Dtos;
using RM.News.Records;
using RM.News.Services;

namespace RM.News.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
             => _newsService = newsService;

        [HttpPost]

        public async Task<OperationOutput> GetNews(GetNewsRecord RequestedData)
            => await _newsService.GetNewsRedis(RequestedData.Adapt<NewsDto>());


        [HttpPost]

        public async Task<OperationOutput> GetNewsByID(GetNewsByIDRecord RequestedData)
            => await _newsService.GetNewsByID(RequestedData.Adapt<NewsDetailDto>());

        [HttpPost]

        public async Task<OperationOutput> GetTagsList()
            => await _newsService.GetTagsList();

        [HttpPost]

        public async Task<OperationOutput> SaveNews(SaveNewsRecord RequestedData)
            => await _newsService.SaveNews(RequestedData.Adapt<NewsDto>());

        [HttpPost]

        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
       => await _newsService.ModelAction(RequestedData.Adapt<NewsDto>());

        [HttpPost]

        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
            => await _newsService.ModelAction(RequestedData.Adapt<NewsDto>());


        [HttpPost]

        public async Task<OperationOutput> SaveTags(TagsDto RequestedData)
           => await _newsService.SaveTags(RequestedData);

        [HttpPost]

        public async Task<OperationOutput> GetReferences()
            => await _newsService.GetReferences();


        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
           => _newsService.GetPathOfResource(fileName);
    }
}
