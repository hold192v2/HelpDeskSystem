namespace Infrastracture.Domain.Entities;

public class Performers: BaseEntity
{
    public List<PerformerInfo> content { get; set; }
    public Pagination pagination { get; set; }
}