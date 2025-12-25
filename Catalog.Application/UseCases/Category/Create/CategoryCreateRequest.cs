using System.ComponentModel;
using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.Category.Create;

public record CategoryCreateRequest(
    string Name,
    int Sla): IRequest<Response>;