
using restful.Auth.Repository;
using System.IdentityModel.Tokens.Jwt;

namespace restful.Auth.Service
{
    public class AuthService : IAuthService
    {

        private readonly IAuthRepository _repository;

        public AuthService(IAuthRepository repositor)
        {
            _repository = repositor;
        }

        public async Task<JwtSecurityToken> GetToken(LoginRequest model)
        {
            return await _repository.GetToken(model);
        }
        public async Task<bool> СreateRole(RoleRequest request)
        {
            return await _repository.СreateRole(request);
        }
    }
}
