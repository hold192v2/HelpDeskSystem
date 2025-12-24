using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.PriorityUpdate;

public class PriorityUpdateHandler: IRequestHandler<PriorityUpdateRequest, Response>
{
    private readonly IPriorityRepository _priorityRepository;

    public PriorityUpdateHandler(IPriorityRepository priorityRepository)
    {
        _priorityRepository = priorityRepository;
    }
    
    public async Task<Response> Handle(PriorityUpdateRequest request, CancellationToken cancellationToken)
    {
        _priorityRepository.UpdatePriority(request.Id, request.Name, request.Sla);
        return new Response("Priority Updated", 200);
    }
}