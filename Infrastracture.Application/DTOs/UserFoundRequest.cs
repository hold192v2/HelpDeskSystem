namespace Infrastracture.Application.DTOs;

public record UserFoundRequest(int? RegionId, int? FilialId, string? Search = "");