using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using restful.Auth.Config;

namespace restful.Auth.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<JwtSecurityToken> GetToken(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.Select(x => new Claim(ClaimTypes.Role, x));
                authClaims.AddRange(role);

                return  GetToken(authClaims);
            }
            return null;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("606c8c855a7ca2e747d9e358e55cb941"));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                 issuer: "http://localhost:7173",
                 audience: "http://localhost:7173",
                 authClaims,
                 expires: DateTime.Now.AddMonths(2),
                 signingCredentials: credentials
                );

            return token;
        }
        public async Task<bool> СreateRole(RoleRequest request)
        {
            var role = new ApplicationRole { Name = request.RoleName };
            var result = await _roleManager.CreateAsync(role);
            if(result != null)
            {
                return true;
            }
            return false;
        }
    }
}
