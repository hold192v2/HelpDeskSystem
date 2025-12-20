using AutoMapper;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Post;

public class CreateNewOfficePostHandler: IRequestHandler<CreateNewOfficePostRequest, Response>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepositoryRepository;
    public CreateNewOfficePostHandler(IOfficeRepository officeRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepositoryRepository)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepositoryRepository =  userRepositoryRepository;
    }
    
    public async Task<Response> Handle(CreateNewOfficePostRequest request, CancellationToken cancellationToken)
    {
        int regionId;
        if (request.RegionId == null)
            regionId = await _userRepositoryRepository.GetUserRegionId(request.UserId);
        else regionId = (int)request.RegionId;
        
        await _officeRepository.AddOfficeAsync(request.City, request.Address, regionId);
        await _unitOfWork.Commit(cancellationToken);
        return new Response("", 200);
    }
}