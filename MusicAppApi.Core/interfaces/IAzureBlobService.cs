using Microsoft.AspNetCore.Http;
using MusicAppApi.Models.Enums;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.Core.Interfaces
{
    public interface IAzureBlobService
    {
        Task<AzureUploadResponse> UploadBlobAsync(IFormFile fileToUpload, string fileName, string containerName);
    }
}