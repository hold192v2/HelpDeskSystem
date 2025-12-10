using AutoMapper;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Application.UseCases.CreateNewOffice.Post;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Patch;

public class CreateNewOfficePatchHandler: IRequestHandler<CreateNewOfficePatchRequest, Response>
{
    private readonly IOffice _office;
    private readonly IMapper _mapper;
    public CreateNewOfficePatchHandler(IOffice office, IMapper mapper) 
    {
        _office = office;
        _mapper = mapper;
    }

    public async Task<Response> Handle(CreateNewOfficePatchRequest request, CancellationToken cancellationToken)
    {
        _office.EditOffice(request.OfficeId, request.City, request.Address);
        return new Response("", 200);
    }
}