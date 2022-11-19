﻿using AutoMapper;
using MemesAPI.Data;
using MemesAPI.Extension;
using MemesAPI.Models.Meme;
using MemesAPI.Models.User;
using MemesAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace MemesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<MemeUser> _userManager;
        private readonly IFileRepository _fileRepository;

        public UserController(ILogger<UserController> logger, AppDBContext context, IMapper mapper, UserManager<MemeUser> userManager, IFileRepository fileRepository)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _fileRepository = fileRepository;
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<Response<ProfileDTO>>> GetProfile(string username)
        {
            Response<ProfileDTO> response = new Response<ProfileDTO>();
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user == null)
                {
                    response.Error = "Not Found";
                    return response;
                }
                var lastMemes = _mapper.Map<List<MemeDTO>>(_context.Memes.Where(m => m.UserId == user.Id).OrderByDescending(m => m.Date).Take(5).ToListAsync());
                var profile = new ProfileDTO(user.UserName, user.Karma, user.profilePic, user.signature, lastMemes);
                response.Data = profile;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "Error retriving profile";
                return response;
            }


        }
        [HttpPut("changepassword")]
        [Authorize]
        public async Task<ActionResult<Response<bool>>> ChangePassword([FromBody]PasswordChange passwordChange)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if(!ModelState.IsValid||passwordChange.NewPassword!=passwordChange.VerifyPassword)
                {
                    response.Error = "Error with passwords";
                    return response;
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                
                    var result = await _userManager.ChangePasswordAsync( user, passwordChange.CurrentPassword, passwordChange.NewPassword);
                if(result.Succeeded)
                {
                    response.IsSuccess=true;
                    response.Data=true;
                    response.Message = "Password Changed!";
                    return response;
                }
                response.Data = false;
                response.Error = "Wrong Current Password ";

                return response;
            }
            catch (Exception ex)
            {

                response.Error = ex.Message;
                response.Message = "Error Changin Password";
                return response;
            }
        }
        [HttpPut("changeprofile")]
        [Authorize]
        public async Task<ActionResult<Response<bool>>> ChangeProfile([FromBody] ProfileChange profile)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (!ModelState.IsValid  )
                {
                    response.Error = "Error with passwords";
                    return response;
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var result = await _userManager.CheckPasswordAsync(user , profile.Password);
                if (!result)
                {
                    response.Data = false;
                    response.Error = "Wrong Password ";
                    return response;
                }
                user.signature= profile.Signature;
                if(profile.IsFile)
                {
                    var imgUrl =await _fileRepository.UploadFile(profile.image);
                    user.profilePic = imgUrl.IsSuccess ? imgUrl.Data : user.profilePic;
                }
                else
                {
                    user.profilePic = profile.profilePic;
                }
                var updateResult=await _userManager.UpdateAsync(user);
                if(updateResult.Succeeded)
                {
                    response.Data = true;
                    response.Message = "Profile Updated!";
                    return response;
                }
                response.Error = "Error updating profile";
                return response;
            }
            catch (Exception ex)
            {

                response.Error = ex.Message;
                response.Message = "Error Changing Profile";
                return response;
            }
        }

    }
}