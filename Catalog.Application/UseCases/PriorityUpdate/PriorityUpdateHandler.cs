using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.PriorityUpdate;

public class PriorityUpdateHandler: IRequestHandler<PriorityUpdateRequest, Response>
{
    private readonly IPriorityRepository _priorityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PriorityUpdateHandler(IPriorityRepository priorityRepository, IUnitOfWork unitOfWork)
    {
        _priorityRepository = priorityRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Response> Handle(PriorityUpdateRequest request, CancellationToken cancellationToken)
    {
        _priorityRepository.UpdatePriority(request.Id, request.Name, request.Sla);
        await _unitOfWork.Commit(cancellationToken);
        return new Response("Priority Updated", 200);
    }
}