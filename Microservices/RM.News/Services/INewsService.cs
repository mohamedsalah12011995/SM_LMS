using RM.Core.Services;
using RM.News.Dtos;

namespace RM.News.Services
{
    public interface INewsService : IBaseService
    {
        Task<OperationOutput> GetReferences();
        Task<OperationOutput> GetNewsRedis(Dtos.NewsDto RequestedData);
        Task<OperationOutput> GetNews(NewsDto RequestedData);
        Task<OperationOutput> GetNewsByID(NewsDetailDto RequestedData);
        Task<OperationOutput> SaveNews(Dtos.NewsDto RequestedData);
        Task<OperationOutput> GetTagsList();
        Task<OperationOutput> SaveTags(TagsDto RequestedData);
        Task<OperationOutput> ModelAction(Dtos.NewsDto RequestedData);
    }
}
