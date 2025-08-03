namespace Domain.Entities;

public class UserSettings
{
    public string? Id { get; set; }
    public int UserId { get; set; }
    public string Language { get; set; } = "English";
    public string Theme { get; set; } = "light";
}