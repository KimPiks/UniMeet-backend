namespace UniMeet.UniversityModule.Application.DTOs;

public class AllowedEmailDomainDto
{
    public required int Id { get; set; }
    public required string Domain { get; set; } = null!;
}