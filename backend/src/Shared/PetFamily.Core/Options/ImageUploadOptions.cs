namespace PetFamily.Core.Options;

public class ImageUploadOptions
{
    public const string IMAGE_UPLAOD_OPTIONS = "ImageUpload";
    
    public string[] AllowedExtensions { get; set; }
    
    public int MaxImageSize { get; set; }
}