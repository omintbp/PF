namespace PetFamily.Application.Options;

public class ImageUploadOptions
{
    public const string IMAGE_UPLAOD_OPTIONS = "ImageUploadOptions";
    
    public string[] AllowedExtensions { get; set; }
    
    public int MaxImageSize { get; set; }
}