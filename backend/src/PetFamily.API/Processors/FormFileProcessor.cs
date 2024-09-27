using PetFamily.Application.DTOs.Shared;

namespace PetFamily.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<FileDto> _files = [];
    
    public IReadOnlyList<FileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var content = file.OpenReadStream();
            var fileName = file.FileName;

            var fileDto = new FileDto(content, fileName);
            
            _files.Add(fileDto);
        }

        return _files;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _files)
        {
            await file.Content.DisposeAsync();
        }
    }
}