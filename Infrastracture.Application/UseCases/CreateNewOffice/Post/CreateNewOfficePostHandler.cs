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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _userRepository;
    public CreateNewOfficePostHandler(IOffice office, IMapper mapper, IUnitOfWork unitOfWork, IUser userRepository)
    {
        _office = office;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository =  userRepository;
    }
    
    public async Task<Response> Handle(CreateNewOfficePostRequest request, CancellationToken cancellationToken)
    {
        int regionId;
        if (request.RegionId == null)
            regionId = _userRepository.GetUserRegionId(request.UserId);
        else regionId = (int)request.RegionId;
        
        _office.AddOffice(request.City, request.Address, regionId);
        await _unitOfWork.Commit(cancellationToken);
        return new Response("", 200);
    }
}