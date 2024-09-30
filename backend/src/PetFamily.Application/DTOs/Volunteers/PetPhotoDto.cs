namespace PetFamily.Application.DTOs.Volunteers;

public class PetPhotoDto
{
    public Guid PhotoId { get; init; }
    
    public string FilePath { get; init; }
    
    public bool IsMain { get; init; }
}