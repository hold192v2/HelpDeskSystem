namespace Infrastracture.Application.DTOs;

public class ContentDTO
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public List<string> category { get; set; }
    public List<string> office { get; set; }
}