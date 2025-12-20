using AutoMapper;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Application.UseCases.CreateNewOffice.Post;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Patch;

public class CreateNewOfficePatchHandler: IRequestHandler<CreateNewOfficePatchRequest, Response>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public CreateNewOfficePatchHandler(IOfficeRepository officeRepository, IMapper mapper,  IUnitOfWork unitOfWork) 
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(CreateNewOfficePatchRequest request, CancellationToken cancellationToken)
    {
        await _officeRepository.EditOfficeAsync(request.OfficeId, request.City, request.Address);
        await _unitOfWork.Commit(cancellationToken);
        return new Response("", 200);
    }
}