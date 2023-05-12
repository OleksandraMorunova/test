using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace restful.Auth.Service
{
    public interface IAuthService
    {
        public Task<JwtSecurityToken> GetToken(LoginRequest model);
        public Task<bool> СreateRole(RoleRequest request);
    }
}
