using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Regions;

public record RegionsRequest(): IRequest<Response>;