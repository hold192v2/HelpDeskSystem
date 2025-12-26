using Catalog.Application.DTOs;
using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.Priorities;

public class PrioritiesHandler: IRequestHandler<PrioritiesRequest, Response>
{
    private readonly IPriorityRepository _priorityRepository;

    public PrioritiesHandler(IPriorityRepository priorityRepository)
    {
        _priorityRepository = priorityRepository;
    }
    
    public async Task<Response> Handle(PrioritiesRequest request, CancellationToken cancellationToken)
    {
        var priorities = await _priorityRepository.GetAllPriorities();
        var result = priorities
            .Select(priority => 
                new PriorityDto(priority.Id, priority.Name, priority.SlaFactor)
            ).ToList();
        
        return new Response("Priorities", 200, result);
    }
}