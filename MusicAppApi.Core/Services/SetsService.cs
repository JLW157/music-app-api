using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MusicAppApi.Core.interfaces.Repositories;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.DTO_s.Sets;
using MusicAppApi.Models.Enums;
using MusicAppApi.Models.Requests;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.Core.Services
{
    public class SetsService : ISetsService
    {
        private ISetsRepository _setsRepository;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<SetsService> _logger;
        private readonly IMapper _mapper;

        public SetsService(ISetsRepository setsRepository, IMapper mapper, IAzureBlobService azureBlobService,
            ILogger<SetsService> logger)
        {
            _setsRepository = setsRepository;
            _azureBlobService = azureBlobService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CreateSetResponse> CreateSet(CreateSetRequestForService createSetRequest)
        {
            CreateSetResponse createSetResponse = new CreateSetResponse()
            {
                Set = new SetDto()
            };

            if (createSetRequest.Poster is not null)
            {

                var response =
                    await _azureBlobService.UploadBlobAsync(createSetRequest.Poster, createSetRequest.Poster.FileName,
                        Constants.AppConstants.AzureBlob.ImageBlobContainerName);

                switch (response.UploadStatus)
                {
                    case AzureUploadStatuses.NameAlreadyExists:
                        {
                            createSetResponse.Set.PosterUrl = response.BlobUrl;
                            _logger.LogError($"Poster with such name already exists. Status - {response.UploadStatus}.");
                            break;
                        }
                    case AzureUploadStatuses.Notuploaded:
                        {
                            createSetResponse.Set.PosterUrl = response.BlobUrl;
                            _logger.LogError($"Poster for set hasn`t uploaded. Status - {response.UploadStatus}.");
                            break;
                        }
                    case AzureUploadStatuses.Uploaded:
                        {
                            createSetResponse.Set.PosterUrl = response.BlobUrl;
                            break;
                        }
                }
            }

            var setCreated = await _setsRepository.CreateSet(new CreateSetRequestForDb()
            {
                Name = createSetRequest.Name,
                PosterUrl = createSetResponse.Set.PosterUrl,
                UserId = createSetRequest.UserId
            });



            if (setCreated != null)
            {
                await _setsRepository.SaveChanges();

                createSetResponse.Set.CreatedDate = DateTime.Now;
                createSetResponse.Set.Name = setCreated.Name;
                createSetResponse.Set.PosterUrl = setCreated.PosterUrl;
                createSetResponse.Set.User = setCreated.User.UserName;

                createSetResponse.IsSuccess = true;
                createSetResponse.Message = "Playlist successfully created.";

                return createSetResponse;
            }

            createSetResponse.IsSuccess = false;
            createSetResponse.Message = "Playlist not created";

            return createSetResponse;
        }

        public async Task<IEnumerable<SetDto>> GetUserSets(Guid userId)
        {
            var sets = await _setsRepository.GetUserSets(userId);

            var res = _mapper.Map<IEnumerable<Set>, IEnumerable<SetDto>>(sets);

            return res;
        }

        public async Task<IEnumerable<SetDto>> GetSetsByUsername(string username)
        {
            var sets = await _setsRepository.GetSetsByUsername(username);

            return _mapper.Map<IEnumerable<Set>, IEnumerable<SetDto>>(sets);
        }

        public async Task<AudioResponse?> AddAudioToSet(AddAudioToSetRequest addAudioToSetRequest)
        {
            var audioId = Guid.Parse(addAudioToSetRequest.AudioId);
            var setId = Guid.Parse(addAudioToSetRequest.SetId);

            var res = await _setsRepository.AddAudioToSet(audioId, setId);


            if (res == null) throw new Exception("Audio not added");

            await _setsRepository.SaveChanges();

            return _mapper.Map<Audio, AudioResponse>(res);
        }

        public async Task<AudioResponse> RemoveAudioFromSet(RemoveAudioFromSetRequest removeAudioRequest)
        {
            var audioId = Guid.Parse(removeAudioRequest.AudioId);
            var setId = Guid.Parse(removeAudioRequest.SetId);

            var res = await _setsRepository.RemoveAudioFromSet(audioId, setId);


            if (res == null) throw new Exception("Audio not removed");

            await _setsRepository.SaveChanges();

            return _mapper.Map<Audio, AudioResponse>(res);
        }
    }
}
