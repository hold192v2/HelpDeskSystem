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
        var filials = await _filialRepository.GetFilialsWithAnalystAsync();
        return new Response("Regions", 200, filials.ToList());
    }
}