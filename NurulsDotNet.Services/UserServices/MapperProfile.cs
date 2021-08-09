using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NurulsDotNet.Data.Models;

namespace NurulsDotNet.Services.UserServices
{
  public static class Mapping
  {
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
      var config = new MapperConfiguration(cfg =>
      {
      // This line ensures that internal properties are also mapped over.
      cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        cfg.AddProfile<MapperProfile>();
      });
      var mapper = config.CreateMapper();
      return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
  }

  public class MapperProfile : Profile
  {
    public MapperProfile()
    {
      // User -> UserAuthResult
      CreateMap<User, UserAuthResult>();

      // UserAuthRequest -> User
      CreateMap<UserAuthRequest, User>();
    }
  }
}
