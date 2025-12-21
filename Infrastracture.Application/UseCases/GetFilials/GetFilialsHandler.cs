using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.GetFilials;

public class GetFilialsHandler : IRequestHandler<GetFilialsRequest, Response>
{
    private readonly IFilialAreaRepository _filialRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetFilialsHandler(IFilialAreaRepository filialRepository, IUserRepository userRepository, IMapper mapper)
    {
        _filialRepository = filialRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Response> Handle(GetFilialsRequest request, CancellationToken cancellationToken)
    {
        var regions = await _filialRepository.GetAllFilials();
        var resultTaskDto = regions.Select( async region =>
        {
            if (region.AnaliticId != null)
            {
                var user = await _userRepository.GetUserByUserId(region.AnaliticId.Value);
                return new FilialDto(region.Id, region.Name, user.Surname, user.Name, user.Patronymic);
            }

            return new FilialDto(region.Id, region.Name, null, null, null);
        }).ToList();
        var resultDto = await Task.WhenAll(resultTaskDto);
        return new Response("Regions", 200, resultDto.ToList());
    }
}