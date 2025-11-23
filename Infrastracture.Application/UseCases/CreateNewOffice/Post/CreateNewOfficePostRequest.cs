using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.CreateNewOffice.Post;

public record CreateNewOfficePostRequest(string City, string Address, int RegionId): IRequest<Response>;