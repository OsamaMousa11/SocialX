using SocialX.Core.Domain.IdentityEntites;
using SocialX.Core.DTO.AuthenticationDTO;
using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IAuthenticationServices
    {

        Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);
        Task<AuthenticationResponse> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

       
        Task<string> AddRoleToUserAsync(AddRoleDTO model);
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        Task<string> DeleteRoleAsync(string roleName);

        // ====== Users ======
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<string> UpdateUserAsync(UpdateUserDTO dto);
        Task<string> DeleteUserAsync(string id);
    }
}
