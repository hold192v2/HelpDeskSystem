using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.SearchUser;

public class SearchUserRequest(string Fullname): IRequest<Response>;