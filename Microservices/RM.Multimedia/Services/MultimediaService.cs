using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Multimedia.Dtos;
using RM.Multimedia.Integrations;
using RM.Multimedia.UnitOfWorks;

using static RM.Multimedia.Dtos.OperationOutput;


namespace RM.Multimedia.Services
{
    public class MultimediaService : BaseService, IMultimediaService
    {

        private string galleryImagesSavePath;
        private string galleryThumbsSavePath;
        private string galleryImagesGetPath;
        private string galleryThumbsGetPath;
        private readonly IUnitOfWork _unitOfWork;


        public MultimediaService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            SetGalleryImagesPath();
        }

        #region HELPER METHOD >> MultimediaService
        void SetGalleryImagesPath()
        {
            galleryImagesSavePath = DirPath + nameof(FilePathRootConst.images);
            galleryThumbsSavePath = DirPath + nameof(FilePathRootConst.icons);
            galleryImagesGetPath = galleryThumbsGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);
        }

        #endregion

        public async Task<OperationOutput> GetVideos(Dtos.MultimediaDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.EntityId = (int)Enums.Entities.VideoGallery;

            if (IsPortal)
                RequestedData.IsActive = true;

            var multimedia = await _unitOfWork.Multimedia.FindAllByPagination(RequestedData.Filteration(),
                                RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate,
                                OrderBy.Descending, x => x.CreatedByNavigation, c => c.UpdatedByNavigation);

            var multimediaDto = multimedia.Data.Adapt<List<Dtos.MultimediaDto>>(MultimediaDto.SelectConfig(RequestUserRole, false));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.VideoGallery, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.MultimediaEntity, multimediaDto),
               new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt(RequestedData.EntityId.Value)),
               new OutputDictionary(OperationOutputKeys.Pagination, multimedia.Pagination));

        }

        public async Task<OperationOutput> GetVideoDetails(Dtos.MultimediaDetailsDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var vedio = _unitOfWork.Multimedia.GetAll()
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted != true);

            if (vedio == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var vedioDto = vedio.Adapt<MultimediaDto>(MultimediaDto.SelectConfig(RequestUserRole, false));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.VideoGallery, vedioDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.MultimediaEntity, vedioDto));

        }


        public async Task<OperationOutput> GetImageGalleries(Dtos.MultimediaDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            RequestedData.EntityId = (int)Enums.Entities.ImageGallery;
            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var multimedia = await _unitOfWork.Multimedia.FindAllByPagination(RequestedData.Filteration(),
                              RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate,
                              OrderBy.Descending, a => a.Attachments, x => x.CreatedByNavigation, c => c.UpdatedByNavigation);

            var multimediaDto = multimedia.Data.Adapt<List<Dtos.MultimediaDto>>(MultimediaDto.SelectConfig(RequestUserRole, true, galleryImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.ImageGallery, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.MultimediaEntity, multimediaDto),
                       new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt(RequestedData.EntityId.Value)),
                       new OutputDictionary(OperationOutputKeys.Pagination, multimedia.Pagination));
        }
        public OperationOutput GetImageByAlbum(Dtos.MultimediaDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var attachmentModel = _unitOfWork.Multimedia.GetAll().Where(x => x.Id == RequestedData.Id)
                .SelectMany(media => media.Attachments.Select(attachment => attachment)).ToList();

            var attachmentDto = attachmentModel.Adapt<List<Attachments>>(Attachments.SelectConfig(galleryImagesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.AttachmentEntity, attachmentDto),
                       new OutputDictionary(OperationOutputKeys.EntityID, attachmentDto.Any() ? Cryptography.AES.Encrypt(attachmentDto[0].EntityId.Value) : null));

        }
        public async Task<OperationOutput> GetAlbumDetails(Dtos.MultimediaDetailsDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var album = await _unitOfWork.Multimedia.GetAll()
                .Include(c => c.Attachments)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted != true);

            if (album == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var albumDto = album.Adapt<MultimediaDto>(MultimediaDto.SelectAlbumDetailsConfig(galleryImagesSavePath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.ImageGallery, albumDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                      new OutputDictionary(OperationOutputKeys.MultimediaEntity, albumDto),
                      new OutputDictionary(OperationOutputKeys.EntityID, albumDto.entityId));

        }
        public async Task<OperationOutput> SaveVideos(Dtos.MultimediaDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.Multimedia multimedia = new();

            if (RequestedData.Id.HasValue)
            {
                multimedia = _unitOfWork.Multimedia.GetById(RequestedData.Id.Value);

                if (multimedia is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(multimedia, RequestedData.UpdateConfig(RequestOwner.Id, isVedio: true));
                _unitOfWork.Multimedia.Update(multimedia);
            }
            else
            {
                RequestedData.Adapt(multimedia, RequestedData.AddConfig(RequestOwner.Id, isVedio: true));
                _unitOfWork.Multimedia.Add(multimedia);
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.Id, Accessor.Get(multimedia.Id)));

        }
        public async Task<OperationOutput> SaveAlbum(Dtos.MultimediaDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.Multimedia multimedia = new();
            Models.Attachment attachment = new();
            List<Attachments> dbItemExistsAttachments;

            if (RequestedData.Id.HasValue)
            {
                multimedia = _unitOfWork.Multimedia.GetById(RequestedData.Id.Value);

                if (multimedia is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                dbItemExistsAttachments = _unitOfWork.Attachment.GetAll().Where(v => v.ItemId == RequestedData.Id)
                    .AsNoTracking().ToList().Adapt<List<Attachments>>();

                var NewImages = RequestedData.ListOfImages.Where(x => !dbItemExistsAttachments.Select(v => v.Id)
                                .Contains(x.Id.HasValue ? x.Id.Value : 0)).ToList().Adapt<List<Dtos.Attachments>>();

                AddAlbumNewImages(RequestedData, NewImages,ref multimedia);
                RemoveAlbumImages(RequestedData, dbItemExistsAttachments);

                RequestedData.Adapt(multimedia, RequestedData.UpdateConfig(RequestOwner.Id, isVedio: false));
                _unitOfWork.Multimedia.Update(multimedia);
            }
            else
            {
                RequestedData.Adapt(multimedia, RequestedData.AddConfig(RequestOwner.Id, isVedio: false));
                AddAlbumNewImages(RequestedData, RequestedData.ListOfImages,ref multimedia);

                _unitOfWork.Multimedia.Add(multimedia);
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Id, Accessor.Get(multimedia.Id)));

        }

        #region HELPER METHODS >> SaveAlbum
        private void AddAlbumNewImages(MultimediaDto RequestedData, List<Attachments> newImages,ref Models.Multimedia multimedia)
        {
            Models.Attachment attachment;
            if (newImages.Any())
            {
                foreach (var image in newImages)
                {
                    attachment = new Models.Attachment();
                    image.Adapt(attachment, image.AddConfig(RequestOwner.Id));
                    attachment.ReferenceId = RequestedData.ReferenceId;
                    attachment.ItemId = multimedia.Id;
                    attachment.Url = Images.SaveSingleImageOnServer(image.Url, null, galleryImagesSavePath, true, null, galleryThumbsSavePath);
                    multimedia.Attachments.Add(attachment);
                }
            }
        }

        private void RemoveAlbumImages(MultimediaDto RequestedData, List<Attachments> dbItemExistsAttachments)
        {
            var mustRemovedImages = dbItemExistsAttachments.Where(x => !RequestedData.ListOfImages
            .Select(v => v.Id).Contains(x.Id)).Adapt<List<Attachments>>();
            var mustRemovedImagesModels = mustRemovedImages.Adapt(new List<Models.Attachment>());

            if (mustRemovedImagesModels.Any())
                _unitOfWork.Attachment.DeleteRange(mustRemovedImagesModels);

        }

        #endregion


        public async Task<OperationOutput> ModelAction(Dtos.MultimediaDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.Multimedia.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.Multimedia.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(d => d.ActivatedBy, RequestOwner.Id)
                        .SetProperty(y => y.ActivatedDate, TransactionDate)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
