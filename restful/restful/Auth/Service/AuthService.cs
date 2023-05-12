using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace restful.Auth.Service
{
    public class AuthService : IAuthService
    {

        private readonly IAuthRpositorye _repository;

        public AuthService(IAuthService service)
        {
            _repository = service;
        }

        public async Task<JwtSecurityToken> GetToken(LoginRequest model)
        {
            return await _repository.GetToken(model);
        }
        public async Task<bool> СreateRole(RoleRequest request)
        {
            return await _repository.CreateRole(request);
        }
    }
}
