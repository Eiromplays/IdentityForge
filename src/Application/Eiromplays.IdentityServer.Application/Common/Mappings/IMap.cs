using AutoMapper;

namespace Eiromplays.IdentityServer.Application.Common.Mappings;

public interface IMap<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}