using LinqKit;

namespace RM.News.Interfaces
{
    public interface IFilterationNews<T>
    {
        ExpressionStarter<T> Filteration(List<int> publishNews);
    }
}
