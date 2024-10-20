namespace PetFamily.Volunteers.Infrastructure.Options;

public class MinioOptions
{
    public const string MINIO = "Minio";
    
    public string Endpoint { get; init; }
    
    public string AccessKey { get; init; }
    
    public string SecretKey { get; init; }
    
    public bool WithSsl { get; init; }
}