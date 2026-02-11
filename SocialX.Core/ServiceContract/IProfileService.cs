using SocialX.Core.DTO.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IProfileService
    {
        Task<ProfileResponse> CreateProfileAsync(
                Guid userId,
                ProfileAddRequest request,
                CancellationToken cancellationToken = default);
        Task<ProfileResponse> UpdateProfileAsync(
         Guid userId,
         ProfileUpdateRequest request,
         CancellationToken cancellationToken = default);
         Task<ProfileResponse> GetProfileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

        Task<ProfileResponse> GetMyProfileAsync(
              Guid userId,
              CancellationToken cancellationToken = default);

        Task DeleteMyProfileAsync(Guid userId);
    }
}
