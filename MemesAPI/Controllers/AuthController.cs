using AutoMapper;
using Google.Apis.Auth;
using MemesAPI.Data;
using MemesAPI.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System;
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
        private readonly JWTManager _jWTManager;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<MemeUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            this.configuration = configuration;
            _jWTManager = new JWTManager(_userManager,configuration);
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDTO>> Register(UserDTO userDTO)
        {
            try{
                var user = new MemeUser();
                user.UserName = userDTO.UserName;
                user.Email=userDTO.Email;
                
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
                string tokenStr = await _jWTManager.GenerateToken(user);
                var response = new AuthResponce
                {
                    UserName = userDTO.UserName,
                    Token = tokenStr,
                    UserId = user.Id,
                    IsAuthSuccessful=true
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went worng");
                return Problem("Login error", statusCode: 500);
            }
        }

       
        [HttpPost("externallogin")]
        public async Task<ActionResult<AuthResponce>> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await _jWTManager.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");
            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new MemeUser { Email = payload.Email, UserName =("User-"+ RandomString(17)) };
                    await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                return BadRequest("Invalid External Authentication.");
            //check for the Locked out account
            var token = await _jWTManager.GenerateToken(user);
            var response = new AuthResponce
            {
                UserName = user.UserName,
                Token = token,
                UserId = user.Id,
                IsAuthSuccessful = true
            };
            return Ok(response);
        }
      
        private string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
