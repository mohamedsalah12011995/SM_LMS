using LinqKit;
using RM.Models;

namespace RM.Permits.Interfaces
{
    public interface IFilterationPermits<T>
    {
        ExpressionStarter<T> Filteration(User user);
    }
}
