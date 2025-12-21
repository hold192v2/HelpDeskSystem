using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.SearchUser;

public record SearchUserRequest(Guid? UserId, string? Role, int? RegionId, int? FilialId, string? Search = ""): IRequest<Response>;