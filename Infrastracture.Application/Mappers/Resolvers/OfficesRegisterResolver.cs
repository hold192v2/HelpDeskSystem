using AutoMapper;
using DTOs;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Mappers.Resolvers;

public class OfficesRegisterResolver : IValueResolver<RegisterIntoInfrastructureDto, User, ICollection<Office>>
{
    public ICollection<Office> Resolve(RegisterIntoInfrastructureDto source, User destination,
        ICollection<Office> destMember,
        ResolutionContext context) => new List<Office> { (Office)context.Items["Office"] };
}
