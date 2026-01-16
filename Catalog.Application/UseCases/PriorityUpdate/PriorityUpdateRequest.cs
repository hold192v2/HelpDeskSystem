using Catalog.Application.DTOs;
using Catalog.Application.HandlerResponse;
using Catalog.Domain.Dtos;
using MediatR;

namespace Catalog.Application.UseCases.PriorityUpdate;

public record PriorityUpdateRequest(List<PriorityUpdateDto> UpdatePriorities): IRequest<Response>;