using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.UserPanel;

public record UserPanelRequest(Guid userId, bool panel): IRequest<Response>;