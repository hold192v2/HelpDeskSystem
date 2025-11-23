using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.UserPanel;

public record UserPanelRequest(Guid UserId, bool Panel): IRequest<Response>;