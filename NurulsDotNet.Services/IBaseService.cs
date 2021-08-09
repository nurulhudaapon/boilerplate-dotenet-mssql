using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NurulsDotNet.Api.Services
{
  public interface IBaseService<T, F>
  {
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetByFilter(F filter);
    Task<T> GetById(int id);
    Task<T> DeleteById(int id);
    Task<T> GetByPublicId(Guid publicId);
    Task<T> Update(T data);
    Task<T> Patch(T data);
    Task<T> Create(T data);
    Task<T> Delete(T data);
  }
}
