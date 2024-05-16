namespace OsanWebsite.Core.Models;

public record Service (string? Id, string Name, string? Description, TimeOnly StartTime, TimeOnly EndTime);
