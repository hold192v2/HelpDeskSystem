using MediatR;
using Infrastracture.Application.HandlerResponse;

namespace Infrastracture.Application.UseCases.UserInfo;

public record UserInfoRequest(Guid UserId): IRequest<Response>;