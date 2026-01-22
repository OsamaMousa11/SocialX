using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.ProfileDto;
using SocialX.Core.Exceptions;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            ILogger<ProfileService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
        }

       
        public async Task<ProfileResponse> CreateProfileAsync(
            Guid userId,
            ProfileAddRequest request,
            CancellationToken cancellationToken = default)
        {
            var profile = _mapper.Map<Domain.Entites.Profile>(request);
            profile.UserId = userId;
            profile.CreatedAt = DateTime.UtcNow;

            if (request.ProfileImage != null)
            {
                profile.ProfileImageUrl =
                    await _fileService.UploadFileAsync(request.ProfileImage, "profiles", cancellationToken);
            }

            if (request.BackgroundImage != null)
            {
                profile.ProfileBackgroundImageUrl =
                    await _fileService.UploadFileAsync(request.BackgroundImage, "profiles", cancellationToken);
            }

            await _unitOfWork.Repository<Domain.Entites.Profile>().AddAsync(profile, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Created new profile for user {UserId}", userId);

            return _mapper.Map<ProfileResponse>(profile);
        }

       
        public async Task<ProfileResponse> UpdateProfileAsync(
            Guid userId,
            ProfileUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var profile = await _unitOfWork.Repository<Domain.Entites.Profile>()
                .FindAsync(p => p.UserId == userId && !p.IsDeleted, cancellationToken);

            if (profile == null)
                throw new NotFoundException("Profile not found");

            _mapper.Map(request, profile);

            if (request.ProfileImageUrl != null)
            {
                profile.ProfileImageUrl =
                    await _fileService.UploadFileAsync(request.ProfileImageUrl, "profiles", cancellationToken);
            }

            if (request.BackgroundImage != null)
            {
                profile.ProfileBackgroundImageUrl =
                    await _fileService.UploadFileAsync(request.BackgroundImage, "profiles", cancellationToken);
            }

            _unitOfWork.Repository<Domain.Entites.Profile>().Update(profile);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("Updated profile for user {UserId}", userId);

            return _mapper.Map<ProfileResponse>(profile);
        }


        public async Task<ProfileResponse> GetProfileByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var profile = await _unitOfWork.Repository<Domain.Entites.Profile>()
                .FindAsync(p => p.UserId == userId && !p.IsDeleted, cancellationToken);

            if (profile == null)
                throw new NotFoundException("Profile not found");

            return _mapper.Map<ProfileResponse>(profile);
        }


        public async Task<ProfileResponse> GetMyProfileAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await GetProfileByUserIdAsync(userId, cancellationToken);
        }
    }
}
