using RM.Core.Services;
using RM.Multimedia.Dtos;

namespace RM.Multimedia.Services
{
    public interface IMultimediaService : IBaseService
    {
        Task<OperationOutput> GetVideos(Dtos.MultimediaDto RequestedData);
        Task<OperationOutput> GetVideoDetails(Dtos.MultimediaDetailsDto RequestedData);
        Task<OperationOutput> GetImageGalleries(Dtos.MultimediaDto RequestedData);
        OperationOutput GetImageByAlbum(Dtos.MultimediaDto RequestedData);
        Task<OperationOutput> GetAlbumDetails(Dtos.MultimediaDetailsDto RequestedData);
        Task<OperationOutput> SaveVideos(Dtos.MultimediaDto RequestedData);
        Task<OperationOutput> SaveAlbum(Dtos.MultimediaDto RequestedData);
        Task<OperationOutput> ModelAction(Dtos.MultimediaDto RequestedData);
    }
}
