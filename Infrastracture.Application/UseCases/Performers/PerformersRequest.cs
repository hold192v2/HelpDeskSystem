using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public record PerformersRequest(int Page, string? Category, Guid? OfficeId, string? Fullname): IRequest<Response>;