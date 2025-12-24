namespace Catalog.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int BasePeriodSla { get; set; }
}