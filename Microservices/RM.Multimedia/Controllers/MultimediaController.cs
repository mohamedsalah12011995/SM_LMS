using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Multimedia.Dtos;
using RM.Multimedia.Records;
using RM.Multimedia.Services;

namespace RM.Multimedia.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MultimediaController : ControllerBase
    {
        private readonly IMultimediaService _multimediaService;

        public MultimediaController(IMultimediaService multimediaService)
            => _multimediaService = multimediaService;


        [HttpPost]

        public async Task<OperationOutput> GetVideos(GetVideosRecord RequestedData)
             => await _multimediaService.GetVideos(RequestedData.Adapt<MultimediaDto>());


        [HttpPost]

        public async Task<OperationOutput> GetImageGalleries(GetImageGalleriesRecord RequestedData)
        => await _multimediaService.GetImageGalleries(RequestedData.Adapt<MultimediaDto>());

        [HttpPost]

        public OperationOutput GetImageByAlbum(GetImageByAlbumRecord RequestedData)
        => _multimediaService.GetImageByAlbum(RequestedData.Adapt<MultimediaDto>());

        [HttpPost]

        public async Task<OperationOutput> SaveVideos(SaveVideosRecord RequestedData)
             => await _multimediaService.SaveVideos(RequestedData.Adapt<MultimediaDto>());

        [HttpPost]

        public async Task<OperationOutput> SaveAlbum(SaveAlbumRecord RequestedData)
            => await _multimediaService.SaveAlbum(RequestedData.Adapt<MultimediaDto>());

        [HttpPost]

        public async Task<OperationOutput> GetAlbumDetails(MultimediaDetailsDto RequestedData)
            => await _multimediaService.GetAlbumDetails(RequestedData);

        [HttpPost]

        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
            => await _multimediaService.ModelAction(RequestedData.Adapt<MultimediaDto>());


        [HttpPost]

        public async Task<OperationOutput> GetVideoDetails(MultimediaDetailsDto RequestedData)
            => await _multimediaService.GetVideoDetails(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
            => await _multimediaService.ModelAction(RequestedData.Adapt<MultimediaDto>());

        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
                   => _multimediaService.GetPathOfResource(fileName);

    }
}
