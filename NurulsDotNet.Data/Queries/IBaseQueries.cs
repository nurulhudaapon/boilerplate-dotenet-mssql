using System.Collections.Generic;
using System.Threading.Tasks;

namespace NurulsDotNet.Data.Queries
{
  public interface IBaseQueries<T, F>
  {
    Task<IEnumerable<T>> Get(F filter);
    Task<T> Update(T data);
    Task<T> Create(T data);
    Task<T> Delete(T data);
  }
}
