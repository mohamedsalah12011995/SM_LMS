using LinqKit;

namespace RM.Surveys.Interfaces
{
    public interface IFilterationSurvey<T>
    {
        ExpressionStarter<T> Filteration(List<int> publishedSurveys);
    }
}
