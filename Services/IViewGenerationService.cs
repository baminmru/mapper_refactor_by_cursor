namespace mapper_refactor.Services;

public interface IViewGenerationService
{
    Task<(string Views, string LoaderScript)> GenerateViewsAsync(string apiName);
    Task<(string Views, string LoaderScript)> GenerateAllViewsAsync();
    Task<string> GenerateViewsForMappingAsync(string mappingName);
} 