using Microsoft.AspNetCore.Http;

namespace PortfolioEngine.Application.Common.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file asynchronously and returns the relative public URL path.
    /// </summary>
    Task<string> UploadFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file given its relative URL path.
    /// </summary>
    Task<bool> DeleteFileAsync(string relativeFilePath);
}