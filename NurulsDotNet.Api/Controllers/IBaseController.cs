using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NurulsDotNet.Api.Controllers
{
  interface IBaseController<T, F>
  {
    Task<IEnumerable<T>> Get(F filter);
    Task<T> GetById(int id);
    Task<T> GetByPublicId(Guid publicId);
    Task<T> Update(T data);
    Task<T> Patch(T data);
    Task<T> Create(T data);
    Task<T> DeleteById(int id);
  }
}
