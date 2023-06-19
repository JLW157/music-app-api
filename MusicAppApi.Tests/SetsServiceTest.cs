using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MusicAppApi.Core.interfaces.Repositories;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Core.Services;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.Requests;

namespace MusicAppApi.Tests
{
    public class SetsServiceTest
    {
        #region AddAudioToSetTests

        [Fact]
        public async Task AddAudioToSetValidAddAudioSetRequest_ShouldReturnCorrectAudioResponse()
        {
            // Arrange
            var addAudioToSetRequest = new AddAudioToSetRequest()
            { AudioId = Guid.NewGuid().ToString(), SetId = Guid.NewGuid().ToString() };

            var expectedAudioResponse = new AudioResponse()
            {
                Id = Guid.Parse(addAudioToSetRequest.AudioId),
                AudioUrl = It.IsAny<string>(),
                Name = It.IsAny<string>()
            };

            var expectedAudio = new Audio()
            {
                Id = expectedAudioResponse.Id,
                AudioUrl = expectedAudioResponse.AudioUrl,
                Name = expectedAudioResponse.Name
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<Audio, AudioResponse>(expectedAudio)).Returns(expectedAudioResponse);

            var mockRepository = new Mock<ISetsRepository>();
            mockRepository
                .Setup(setsRepo
                    => setsRepo.AddAudioToSet(Guid.Parse(addAudioToSetRequest.AudioId),
                        Guid.Parse(addAudioToSetRequest.SetId)))
                .Returns(Task.FromResult(expectedAudio)!);

            var setsSerrvice = new SetsService(mockRepository.Object, mockMapper.Object,
                new Mock<IAzureBlobService>().Object, null);

            // Act
            var actual = await setsSerrvice.AddAudioToSet(addAudioToSetRequest);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<AudioResponse>(actual);
        }

        [Fact]
        public async Task AddAudioToSetInvalidAddAudioSetRequest_ShouldThrowException()
        {
            // Arrange
            var addAudioToSetRequest = new AddAudioToSetRequest()
            { AudioId = Guid.NewGuid().ToString(), SetId = Guid.NewGuid().ToString() };

            var expectedAudioResponse = new AudioResponse()
            {
                Id = Guid.Parse(addAudioToSetRequest.AudioId),
                AudioUrl = It.IsAny<string>(),
                Name = It.IsAny<string>()
            };

            var expectedAudio = new Audio()
            {
                Id = expectedAudioResponse.Id,
                AudioUrl = expectedAudioResponse.AudioUrl,
                Name = expectedAudioResponse.Name
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<Audio, AudioResponse>(expectedAudio)).Returns(expectedAudioResponse);

            var mockRepository = new Mock<ISetsRepository>();
            mockRepository
                .Setup(setsRepo
                    => setsRepo.AddAudioToSet(Guid.Parse(addAudioToSetRequest.AudioId),
                        Guid.Parse(addAudioToSetRequest.SetId)))
                .Returns(Task.FromResult<Audio?>(null));

            var setsSerrvice = new SetsService(mockRepository.Object, mockMapper.Object,
                new Mock<IAzureBlobService>().Object, null);

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var actual = await setsSerrvice.AddAudioToSet(addAudioToSetRequest);
            });
        }

        #endregion

        #region RemoveAudioFromSetTests

        [Fact]
        public async Task RemoveAudioFromSetValidRemoveAudioFromSetRequest_ShouldReturnCorrectAudioResponse()
        {
            // Arrange
            var addAudioToSetRequest = new RemoveAudioFromSetRequest()
                { AudioId = Guid.NewGuid().ToString(), SetId = Guid.NewGuid().ToString() };

            var expectedAudioResponse = new AudioResponse()
            {
                Id = Guid.Parse(addAudioToSetRequest.AudioId),
                AudioUrl = It.IsAny<string>(),
                Name = It.IsAny<string>()
            };

            var expectedAudio = new Audio()
            {
                Id = expectedAudioResponse.Id,
                AudioUrl = expectedAudioResponse.AudioUrl,
                Name = expectedAudioResponse.Name
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<Audio, AudioResponse>(expectedAudio)).Returns(expectedAudioResponse);

            var mockRepository = new Mock<ISetsRepository>();
            mockRepository
                .Setup(setsRepo
                    => setsRepo.RemoveAudioFromSet(Guid.Parse(addAudioToSetRequest.AudioId),
                        Guid.Parse(addAudioToSetRequest.SetId)))
                .Returns(Task.FromResult(expectedAudio)!);

            var setsSerrvice = new SetsService(mockRepository.Object, mockMapper.Object,
                new Mock<IAzureBlobService>().Object, null);

            // Act
            var actual = await setsSerrvice.RemoveAudioFromSet(addAudioToSetRequest);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<AudioResponse>(actual);
        }

        [Fact]
        public async Task RemoveAudioFromSetValidRemoveAudioFromSetRequest_ShouldThrowException()
        {
            // Arrange
            var addAudioToSetRequest = new RemoveAudioFromSetRequest()
                { AudioId = Guid.NewGuid().ToString(), SetId = Guid.NewGuid().ToString() };

            var expectedAudioResponse = new AudioResponse()
            {
                Id = Guid.Parse(addAudioToSetRequest.AudioId),
                AudioUrl = It.IsAny<string>(),
                Name = It.IsAny<string>()
            };

            var expectedAudio = new Audio()
            {
                Id = expectedAudioResponse.Id,
                AudioUrl = expectedAudioResponse.AudioUrl,
                Name = expectedAudioResponse.Name
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<Audio, AudioResponse>(expectedAudio)).Returns(expectedAudioResponse);

            var mockRepository = new Mock<ISetsRepository>();
            mockRepository
                .Setup(setsRepo
                    => setsRepo.RemoveAudioFromSet(Guid.Parse(addAudioToSetRequest.AudioId),
                        Guid.Parse(addAudioToSetRequest.SetId)))
                .Returns(Task.FromResult<Audio?>(null));

            var setsSerrvice = new SetsService(mockRepository.Object, mockMapper.Object,
                new Mock<IAzureBlobService>().Object, null);

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var actual = await setsSerrvice.RemoveAudioFromSet(addAudioToSetRequest);
            });
        }
        #endregion
    }
}
