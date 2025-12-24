using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.Priorities;

public record PrioritiesRequest(): IRequest<Response>;