namespace DTOs;

public record  RegisterIntoInfrastructureDto(Guid Id, string Name,  string Surname, string Patronymic, 
    string Email, int RoleId, Guid OfficeId, int RegionId, string SystemId);