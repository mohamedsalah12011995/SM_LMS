using LinqKit;

namespace RM.DynamicForms.Interfaces
{
    public interface IFilterationForClone<T>
    {
        ExpressionStarter<T> FilterationForClone();
    }
}
