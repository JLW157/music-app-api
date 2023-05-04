using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MusicAppApi.Core.Interfaces;
using MusicAppApi.Models.Enums;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.Core.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _configuration;

        public async Task<AzureUploadResponse> UploadBlobAsync(IFormFile fileToUpload, string fileName, string containerName)
        {
            try
            {
                // to key vault (!)
                var connStr =
                    "";
                BlobServiceClient blobServiceClient = new BlobServiceClient(connStr);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                if (await blobClient.ExistsAsync())
                    return new AzureUploadResponse() { UploadStatus = AzureUploadStatuses.NameAlreadyExists, BlobUrl = blobClient.Uri.ToString() };


                using var stream = fileToUpload.OpenReadStream();

                var res = await blobClient.UploadAsync(stream,
                    new BlobUploadOptions()
                    { HttpHeaders = new BlobHttpHeaders { ContentType = fileToUpload.ContentType } });

                return new AzureUploadResponse()
                { BlobUrl = blobClient.Uri.ToString(), UploadStatus = AzureUploadStatuses.Uploaded };
            }
            catch (Exception e)
            {
                return new AzureUploadResponse() { UploadStatus = AzureUploadStatuses.Notuploaded };
            }
        }
    }
}
