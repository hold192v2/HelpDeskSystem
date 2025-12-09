using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.SearchUser;

public record SearchUserRequest(string Fullname, Guid? UserId): IRequest<Response>;