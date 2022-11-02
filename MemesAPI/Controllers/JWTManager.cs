using Google.Apis.Auth;
using MemesAPI.Data;
using MemesAPI.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemesAPI.Controllers
{
    public class JWTManager
    {
        private readonly UserManager<MemeUser> _userManager;
        private readonly IConfiguration configuration;

        public JWTManager(UserManager<MemeUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<string> GenerateToken(MemeUser user)
        {
            try
            {
                var securitySecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
                var credentials = new SigningCredentials(securitySecret, SecurityAlgorithms.HmacSha256);
                var roles = await _userManager.GetRolesAsync(user);
                var userClaims =  await _userManager.GetClaimsAsync(user);
                var rolesClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }.Union(userClaims).Union(rolesClaims);

                var token = new JwtSecurityToken(
                    issuer: configuration["JwtSettings:Issuer"],
                    audience: configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(int.Parse(configuration["JwtSettings:Duration"])),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { configuration["Authentication:Google:ClientId"] }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }
    }
}
