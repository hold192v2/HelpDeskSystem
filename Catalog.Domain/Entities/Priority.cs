namespace Catalog.Domain.Entities;

public class Priority
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double slaFactor  { get; set; }
}