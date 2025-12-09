using AutoMapper;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Post;

public class CreateNewOfficePostHandler: IRequestHandler<CreateNewOfficePostRequest, Response>
{
    private readonly IOffice _office;
    private readonly IMapper _mapper;
    public CreateNewOfficePostHandler(IOffice office, IMapper mapper) 
    {
        _office = office;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(CreateNewOfficePostRequest request, CancellationToken cancellationToken)
    {
        _office.AddOffice(request.City, request.Address, request.RegionId);
        return new Response("", 200);
    }
}