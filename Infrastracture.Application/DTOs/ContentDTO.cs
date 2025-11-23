namespace Infrastracture.Application.DTOs;

public class ContentDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public List<string> Category { get; set; }
    public List<string> Office { get; set; }
}