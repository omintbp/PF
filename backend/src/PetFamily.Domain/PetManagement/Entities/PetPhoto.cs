using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Domain.PetManagement.Entities;

public class PetPhoto : Shared.Entity<PetPhotoId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    public const int MAX_PATH_LENGTH = 8000;
    private PetPhoto(PetPhotoId id) 
        : base(id)
    {
        
    }
    
    private PetPhoto(PetPhotoId id, string path, bool isMain) : base(id)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }

    public static Result<PetPhoto, Error> Create(PetPhotoId id, string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Length > MAX_PATH_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(path));
        
        var petPhoto = new PetPhoto(id, path, isMain);

        return petPhoto;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }
}