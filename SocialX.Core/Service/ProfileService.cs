using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.ProfileDto;
using SocialX.Core.Exceptions;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;
using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            if (userId == Guid.Empty)
                throw new BadRequestException("User ID cannot be empty");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _unitOfWork.Repository<ApplicationUser>()
                    .GetByIdAsync(userId);

                if (user == null)
                    throw new NotFoundException("User not found");

                var exists = await _unitOfWork.Repository<Domain.Entites.Profile>()
                    .ExistsAsync(p => p.UserId == userId && !p.IsDeleted, cancellationToken);

                if (exists)
                    throw new ConflictException("Profile already exists");

                var profile = _mapper.Map<Domain.Entites.Profile>(request);
                profile.UserId = userId;
               
                profile.IsDeleted = false; 

                if (request.ProfileImg != null)
                {
                    profile.ProfileImageUrl = await _fileService.UploadFileAsync(
                        request.ProfileImg,
                        "profiles",
                        cancellationToken);
                }

                if (request.ProfileBackground != null)
                {
                    profile.ProfileBackgroundImageUrl = await _fileService.UploadFileAsync(
                        request.ProfileBackground,
                        "profiles",
                        cancellationToken);
                }

                await _unitOfWork.Repository<Domain.Entites.Profile>()
                    .AddAsync(profile, cancellationToken);

                
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation(
                    "Profile created successfully. ProfileId: '{ProfileId}', UserId: '{UserId}'",
                    profile.Id, userId);

                return _mapper.Map<ProfileResponse>(profile);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }




        public async Task<ProfileResponse> UpdateProfileAsync(
       Guid userId,
       ProfileUpdateRequest request,
       CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new BadRequestException("User ID cannot be empty");

            var profile = await _unitOfWork.Repository<Domain.Entites.Profile>()
                .FindAsync(p => p.UserId == userId && !p.IsDeleted);

            if (profile == null)
                throw new NotFoundException("Profile not found");

            _mapper.Map(request, profile);
            profile.UpdatedAt = DateTime.UtcNow;
            if (request.ProfileImageUrl != null)
            {
                var newImageUrl = await _fileService.UploadFileAsync(
                    request.ProfileImageUrl,
                    "profiles",
                    cancellationToken);

                if (!string.IsNullOrEmpty(profile.ProfileImageUrl))
                {
                    await _fileService.DeleteFileAsync(
                        profile.ProfileImageUrl,
                        cancellationToken);
                }

                profile.ProfileImageUrl = newImageUrl;
            }

            if (request.BackgroundImage != null)
            {
                var newBgUrl = await _fileService.UploadFileAsync(
                    request.BackgroundImage,
                    "profiles",
                    cancellationToken);

                if (!string.IsNullOrEmpty(profile.ProfileBackgroundImageUrl))
                {
                    await _fileService.DeleteFileAsync(
                        profile.ProfileBackgroundImageUrl,
                        cancellationToken);
                }

                profile.ProfileBackgroundImageUrl = newBgUrl;
            }

            _unitOfWork.Repository<Domain.Entites.Profile>().Update(profile);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation(
                "Profile updated successfully. ProfileId: '{ProfileId}', UserId: '{UserId}'",
                profile.Id, userId);

            return _mapper.Map<ProfileResponse>(profile);
        }


        public async Task<ProfileResponse> GetProfileByUserIdAsync(Guid userId,CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new BadRequestException("User ID cannot be empty");

            var profile = await _unitOfWork.Repository<Domain.Entites.Profile>()
        .FindAsync(
            p => p.UserId == userId && !p.IsDeleted,
            includeProperties: "User",
            cancellationToken);


            if (profile == null)
                throw new NotFoundException("Profile not found");
            profile.IsDeleted = true;
            profile.UpdatedAt = DateTime.UtcNow;

            return _mapper.Map<ProfileResponse>(profile);
        }



        public async Task<ProfileResponse> GetMyProfileAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await GetProfileByUserIdAsync(userId, cancellationToken);
        }

        public async Task DeleteMyProfileAsync(Guid userId)
        {
            var profile = await _unitOfWork.Repository<Domain.Entites.Profile>()
                .FindAsync(p => p.UserId == userId && !p.IsDeleted);

            if (profile == null)
                throw new NotFoundException("Profile not found");

            profile.IsDeleted = true;
            profile.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Domain.Entites.Profile>().Update(profile);
            await _unitOfWork.CompleteAsync();
        }
    }
}
