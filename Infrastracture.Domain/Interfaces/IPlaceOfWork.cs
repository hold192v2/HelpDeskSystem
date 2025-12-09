using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IPlaceOfWork: IBaseOperationRepository<PlaceOfWork>
{
    Task<List<PlaceOfWork>> GetByUserId(Guid userId);
    Task<List<PlaceOfWork>> GetByOfficeId(Guid officeId);
    
}