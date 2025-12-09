using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Patch;

public record CreateNewOfficePatchRequest(Guid OfficeId, string City, string Address): IRequest<Response>;