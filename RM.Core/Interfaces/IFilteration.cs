
using LinqKit;

namespace RM.Core.Interfaces
{
    public interface IFilteration<T>
    {
        ExpressionStarter<T> Filteration();
    }

}
