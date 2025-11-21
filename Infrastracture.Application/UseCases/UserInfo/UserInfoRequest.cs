using MediatR;
using Infrastracture.Application.HandlerResponse;

namespace Infrastracture.Application.UseCases.UserInfo;

public record UserInfoRequest(Guid userId): IRequest<Response>;