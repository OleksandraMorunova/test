using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace restful.Auth.Repository
{
    public interface IAuthRepository
    {
        public Task<JwtSecurityToken> GetToken(LoginRequest model);
        public Task<bool> СreateRole(RoleRequest request);
    }
}
