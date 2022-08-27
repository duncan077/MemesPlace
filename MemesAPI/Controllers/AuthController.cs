using AutoMapper;
using MemesAPI.Data;
using MemesAPI.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemesAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<MemeUser> _userManager;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<MemeUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try{
                var user = _mapper.Map<MemeUser>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(result); }
            catch(Exception e)
            {
                _logger.LogError(e, $"Something went worng");
                return Problem("Register error", statusCode: 500);
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponce>> Login(LoginUserDTO userDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userDTO.UserName);
                var passwordValid = await _userManager.CheckPasswordAsync( user, userDTO.Password);
                if (user == null||!passwordValid)
                    return Unauthorized(userDTO);
                string tokenStr = await GenerateToken(user);
                var response = new AuthResponce
                {
                    UserName = userDTO.UserName,
                    Token = tokenStr,
                    UserId = user.Id
                };
                return Accepted(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went worng");
                return Problem("Login error", statusCode: 500);
            }
        }

        private async Task<string> GenerateToken(MemeUser user)
        {
            var securitySecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
            var credentials = new SigningCredentials(securitySecret, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }.Union(rolesClaims);
        
            var token =new JwtSecurityToken(
                issuer: configuration["JwtSettings.Issuer"],
                audience: configuration["JwtSettings.Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings.Duration"])),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
