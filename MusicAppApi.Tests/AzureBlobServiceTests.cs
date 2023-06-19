using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using MusicAppApi.Core.Services;
using MusicAppApi.Models.Enums;
using Xunit;

namespace MusicAppApi.Tests
{
    public class AzureBlobServiceTests
    {
        //[Fact]
        //public async Task UploadBlobAsyncWithValidArgs_ShouldReturnUploadedStatusAndBlobUrl()
        //{
        //    // Arrange
        //    var fileMock = new Mock<IFormFile>();
        //    var configurationMock = new Mock<IConfiguration>();
        //    var blobContainerClientMock = new Mock<BlobContainerClient>();
        //    var blobClientMock = new Mock<BlobClient>();

        //    // Set up necessary mocks
        //    fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        //    fileMock.Setup(f => f.ContentType).Returns("image/jpeg");

        //    configurationMock.Setup(c => c["AzureBlobStorageConnectionString"])
        //        .Returns("DefaultEndpointsProtocol=https;AccountName=audiostorageweb123;AccountKey=yS89cvaxaLmysCNRQP6BUJd8duqBuIA1cv0RYaTRsD/ScBLznx8Rsifp13mnLRhTV/JUCdSriMPR+AStRW/yww==;EndpointSuffix=core.windows.net");

        //    var blobServiceClientMock = new Mock<BlobServiceClient>(configurationMock.Object["AzureBlobStorageConnectionString"]);
        //    blobServiceClientMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>()))
        //        .Returns(blobContainerClientMock.Object);

        //    blobContainerClientMock.Setup(c => c.GetBlobClient(It.IsAny<string>()))
        //        .Returns(blobClientMock.Object);

        //    blobContainerClientMock.Setup(b => b.ExistsAsync()); // Mocking the ExistsAsync method to return a response with a true result

        //    var azureBlobService = new AzureBlobService(configurationMock.Object);

        //    // Act
        //    var result = await azureBlobService.UploadBlobAsync(fileMock.Object, "filename.jpg", "container");

        //    // Assert
        //    Assert.Equal(AzureUploadStatuses.NameAlreadyExists, result.UploadStatus);
        //    Assert.StartsWith("https://", result.BlobUrl);
        //}

        //public static BlobContainerClient GetBlobContainerClientMock()
        //{
        //    var mock = new Mock<BlobContainerClient>();

        //    mock.Setup(expression: x => x.ExistsAsync())
        //        .Returns(Task.FromResult(It.IsAny<Response<bool>>()));

           

        //    return mock.Object;
        //}
    }
}