using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Domain.Entities;

public class PetPhoto : SharedKernel.Entity<PetPhotoId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    public const int MAX_PATH_LENGTH = 8000;
    private PetPhoto(PetPhotoId id) 
        : base(id)
    {
        
    }
    
    private PetPhoto(PetPhotoId id, FilePath filePath, bool isMain) : base(id)
    {
        FilePath = filePath;
        IsMain = isMain;
    }
    
    public FilePath FilePath { get; private set; }
    
    public bool IsMain { get; private set; }
    
    public static Result<PetPhoto, Error> Create(PetPhotoId id, FilePath path, bool isMain)
    {
        var petPhoto = new PetPhoto(id, path, isMain);

        return petPhoto;
    }

    public void SetAsMain()
    {
        IsMain = true;
    }

    public void SetAsNotMain()
    {
        IsMain = false;
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