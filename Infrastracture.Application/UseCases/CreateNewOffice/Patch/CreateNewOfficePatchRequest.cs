using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Patch;

public record CreateNewOfficePatchRequest(string OfficeId, string City, string Address): IRequest<Response>;