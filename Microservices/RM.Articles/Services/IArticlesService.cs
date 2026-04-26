using RM.Articles.Dtos;

namespace RM.Articles.Services
{
    public interface IArticlesService
    {
        Task<OperationOutput> GetArticlesList(Dtos.Articles RequestedData);
        Task<OperationOutput> GetArticleDetails(Dtos.Articles RequestedData);
        Task<OperationOutput> SaveArticle(Dtos.Articles RequestdData);
        OperationOutput ModelActions(Dtos.Articles RequestedData);
    }
}
