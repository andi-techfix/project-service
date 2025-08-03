namespace Domain.Entities;

public class Project
{
    public string? Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Chart> Charts { get; set; } = [];
}