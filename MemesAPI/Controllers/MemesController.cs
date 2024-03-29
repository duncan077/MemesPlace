﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MemesAPI.Data;
using Microsoft.AspNetCore.Authorization;
using MemesAPI.Extension;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MemesAPI.Models.Meme;
using Microsoft.CodeAnalysis;
using MemesAPI.Repository.Interface;
using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System.Numerics;

namespace MemesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class MemesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly ILogger<MemesController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<MemeUser> _userManager;
        private readonly IFileRepository fileRepository;
        readonly IServiceScopeFactory _serviceScopeFactory;
        public MemesController(AppDBContext context, ILogger<MemesController> logger, IMapper mapper, UserManager<MemeUser> userManager, IFileRepository fileRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            this.fileRepository = fileRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: api/Memes
        [HttpGet]
        
        [AllowAnonymous]
                
        public async Task<ActionResult<IEnumerable<Response<MemeDTO>>>> GetMemes([FromQuery] MemeParameters memeParameters)
        {
            return await GetMemesMethod(memeParameters);
        }
        [HttpGet("auth")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Response<MemeDTO>>>> GetMemesAuth([FromQuery] MemeParameters memeParameters)
        {
            return await GetMemesMethod(memeParameters);
        }

        private async Task<ActionResult<IEnumerable<Response<MemeDTO>>>> GetMemesMethod(MemeParameters memeParameters)
        {
            List<Response<MemeDTO>> response = new List<Response<MemeDTO>>();
            try
            {
                var memes = new List<MemeDTO>();
                if (_context.Memes == null)
                {
                    return NotFound(response);
                }
                if (memeParameters.popular)
                {
                    memes = await GetMemesTrendQuery(memeParameters);
                }
                else
                {
                    memes = await GetMemesQuery(memeParameters);
                }
               
               
                foreach (var meme in memes)
                {

                    

                    var responseDTO = new Response<MemeDTO>() { IsSuccess = true, Data = meme, Message = "Found" };
                    response.Add(responseDTO);
                }


                return Ok(response);
            }
            catch (Exception ex)
            {

                response.Add(new Response<MemeDTO>() { Error = ex.Message, Message = "Error getting memes" });
                return response;
            }
            
        }

        private async Task<List<MemeDTO>> GetMemesQuery(MemeParameters memeParameters)
        {
            var memes = new List<MemeDTO>();
            if (memeParameters.name == "" && memeParameters.tag == "")
            {
                memes = await _context.Memes.AsNoTracking().Include(t => t.Tags).Include(l => l.Likes).Include(u => u.User).Select(meme => new MemeDTO
                {
                    Id = meme.Id,
                    Name = meme.Name,
                    Description = meme.Description,
                    URLIMG = meme.URLIMG,
                    imgProfile = meme.User.profilePic,
                    IsVideo = meme.IsVideo,
                    Format = meme.Format,
                    UserName = meme.UserName,
                    Date = meme.Date,
                    Tags = meme.Tags.Select(x => x.Name).ToList(),
                    like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                    likeCount = meme.Likes.Count


                }).OrderByDescending(m => m.Date).Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize).Take(memeParameters.PageSize).ToListAsync();
            }
            else
            {
                if (memeParameters.name != "")
                {
                    memes = await _context.Memes.AsNoTracking()
                       .Include(t => t.Tags).Include(l => l.Likes).Include(u => u.User).Select(meme => new MemeDTO
                       {
                           Id = meme.Id,
                           Name = meme.Name,
                           Description = meme.Description,
                           URLIMG = meme.URLIMG,
                           imgProfile = meme.User.profilePic,
                           IsVideo = meme.IsVideo,
                           Format = meme.Format,
                           UserName = meme.UserName,
                           Date = meme.Date,
                           Tags = meme.Tags.Select(x => x.Name).ToList(),
                           like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                           likeCount = meme.Likes.Count


                       })
                       .Where(m => m.Name.ToLower().Contains(memeParameters.name.ToLower()))
                       .OrderByDescending(m => m.Date)
                       .Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize)
                       .Take(memeParameters.PageSize).ToListAsync();
                }
                else
                {
                    memes = await _context.Memes.AsNoTracking().Include(t => t.Tags).Include(l => l.Likes).Include(u => u.User).Select(meme => new MemeDTO
                    {
                        Id = meme.Id,
                        Name = meme.Name,
                        Description = meme.Description,
                        URLIMG = meme.URLIMG,
                        imgProfile = meme.User.profilePic,
                        IsVideo = meme.IsVideo,
                        Format = meme.Format,
                        UserName = meme.UserName,
                        Date = meme.Date,
                        Tags = meme.Tags.Select(x => x.Name).ToList(),
                        like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                        likeCount = meme.Likes.Count


                    })
                       .Where(m => m.Tags.Any(t => t.ToLower().Contains(memeParameters.tag.ToLower())))
                       .OrderByDescending(m => m.Date)
                       .Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize)
                       .Take(memeParameters.PageSize).ToListAsync();
                }
            }

            return memes;
        }
        private async Task<List<MemeDTO>> GetMemesTrendQuery(MemeParameters memeParameters)
        {
            var memes = new List<MemeDTO>();
            if (memeParameters.name == "" && memeParameters.tag == "")
            {
                memes = await _context.Memes.AsNoTracking().Include(t => t.Tags).Include(l => l.Likes).Include(u => u.User).Select(meme => new MemeDTO
                {
                    Id = meme.Id,
                    Name = meme.Name,
                    Description = meme.Description,
                    URLIMG = meme.URLIMG,
                    imgProfile = meme.User.profilePic,
                    IsVideo = meme.IsVideo,
                    Format = meme.Format,
                    UserName = meme.UserName,
                    Date = meme.Date,
                    Tags = meme.Tags.Select(x => x.Name).ToList(),
                    like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                    likeCount = meme.Likes.Count


                }).OrderByDescending(m => m.likeCount).Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize).Take(memeParameters.PageSize).ToListAsync();
            }
            else
            {
                if (memeParameters.name != "")
                {
                    memes = await _context.Memes.AsNoTracking()
                       .Include(t => t.Tags).Include(l=>l.Likes).Include(u=>u.User).Select(meme => new MemeDTO
                       {
                           Id = meme.Id,
                           Name = meme.Name,
                           Description = meme.Description,
                           URLIMG = meme.URLIMG,
                           imgProfile = meme.User.profilePic,
                           IsVideo = meme.IsVideo,
                           Format = meme.Format,
                           UserName = meme.UserName,
                           Date = meme.Date,
                           Tags = meme.Tags.Select(x => x.Name).ToList(),
                           like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                           likeCount = meme.Likes.Count


                       })
                       .Where(m => m.Name.ToLower().Contains(memeParameters.name.ToLower()))
                       .OrderByDescending(m => m.likeCount)
                       .Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize)
                       .Take(memeParameters.PageSize).ToListAsync();
                }
                else
                {
                    memes = await _context.Memes.AsNoTracking()
                       .Include(t => t.Tags).Include(l => l.Likes).Include(u => u.User).Select(meme => new MemeDTO
                       {
                           Id = meme.Id,
                           Name = meme.Name,
                           Description = meme.Description,
                           URLIMG = meme.URLIMG,
                           imgProfile = meme.User.profilePic,
                           IsVideo = meme.IsVideo,
                           Format = meme.Format,
                           UserName = meme.UserName,
                           Date = meme.Date,
                           Tags = meme.Tags.Select(x => x.Name).ToList(),
                           like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                           likeCount = meme.Likes.Count


                       })
                       .Where(m => m.Tags.Any(t => t.ToLower().Contains(memeParameters.tag.ToLower())))
                       .OrderByDescending(m => m.likeCount)
                       .Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize)
                       .Take(memeParameters.PageSize).ToListAsync();
                }
            }

            return memes;
        }

        // GET: api/Memes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Response<MemeDTO>>> GetMeme(int id)
        {
            return await GetMemeMethod(id);
        }
        [HttpGet("auth/{id}")]
        [Authorize]
        public async Task<ActionResult<Response<MemeDTO>>> GetMemeAuth(int id)
        {
            return await GetMemeMethod(id);
        }

        private async Task<ActionResult<Response<MemeDTO>>> GetMemeMethod(int id)
        {
            var response = new Response<MemeDTO>();
            try
            {
                if (_context.Memes == null)
                {
                    return NotFound(response);
                }
                var meme = await _context.Memes.AsNoTracking().Include(t => t.Tags).Include(l => l.Likes).Include(u => u.User).Select(meme => new MemeDTO
                {
                    Id = meme.Id,
                    Name = meme.Name,
                    Description = meme.Description,
                    URLIMG = meme.URLIMG,
                    imgProfile = meme.User.profilePic,
                    IsVideo = meme.IsVideo,
                    Format = meme.Format,
                    UserName = meme.UserName,
                    Date = meme.Date,
                    Tags = meme.Tags.Select(x => x.Name).ToList(),
                    like = meme.Likes.Any(x => x.UserName == User.Identity.Name),
                    likeCount = meme.Likes.Count


                }
                ).FirstAsync(m=>m.Id==id);

                if (meme == null)
                {
                    return NotFound(response);
                }
               
              
                response.IsSuccess = true;
                response.Data= meme;
                return response;

            }
            catch (Exception ex)
            {
                response.Error=ex.Message;
                response.Message = "Error retriving meme";
                return response;

            }
           
            
        }

        [HttpPost("like/{id}")]
        [Authorize]
        public async Task<ActionResult<Response<bool>>> LikeMeme(int id)
        {
            try
            {
                if (_context.Memes == null)
                {
                    return NotFound();
                }
                Response<bool> response = new Response<bool> { Data = false };
                var meme = await _context.Memes.Include(m => m.Likes).FirstAsync(m => m.Id == id);
                MemeUser user = (MemeUser)_context.Users.Where(x => x.UserName.Equals(User.Identity.Name)).First();

                if (meme == null)
                {
                    return NotFound(response);
                }
                var like = meme.Likes.Any(z => z.UserName == User.Identity.Name);
                if (like)
                {

                    _context.MemeLike.Remove(_context.MemeLike.First(m => m.UserName == User.Identity.Name));
                    _context.SaveChanges();
                    response.Data = false;
                }
                else
                {
                    var liked = new MemeLike();
                    liked.UserName = User.Identity.Name;
                    liked.MemeId = id;
                    liked.DateTime = DateTime.UtcNow;
                    _context.MemeLike.Add(liked);

                    _context.SaveChanges();
                    response.Data = true;
                }
                
                response.IsSuccess = true;
                response.Message = "Like/Unlike successfully";

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new Response<bool> { Data = false, Message = "Error on like post", Error = ex.Message };
                return BadRequest(response);
                
            }
            
        }

        // POST: api/Memes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("upload")]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)]
        public async Task<ActionResult<List<Response<MemeDTO>>>> PostMeme(List<MemeAddDTO> memeDTO)
        {
           
          if (_context.Memes == null)
          {
              return Problem("Entity set 'AppDBContext.Memes'  is null.");
          }
            try
            {
                var memeResponse = new List<Response<MemeDTO>>();
                foreach (var item in memeDTO)
                {
                    Response<UploadResult> upload= new Response<UploadResult>();
                    var meme = _mapper.Map<Meme>(item);
                    meme.UserName = User.Identity.Name;
                    meme.UserId = User.Claims.First(c => c.Type == "uid").Value;
                    meme.Date = DateTime.UtcNow;
                    if (item.IsFile)
                    {
                         upload = await fileRepository.UploadFile(item.Imgfile);
                        if (upload.IsSuccess)
                        {
                            meme.URLIMG = upload.Data.URL;
                                  meme.Format = upload.Data.Format;
                            meme.IsVideo= upload.Data.IsVideo;
                        }
                        
                    }
                    else
                    {
                        meme.URLIMG = item.URLIMG;
                    }
                    if (meme.URLIMG == "" || !upload.IsSuccess)
                    {
                        _logger.LogInformation("Upload Failed");
                        memeResponse.Add(new Response<MemeDTO> { IsSuccess = false, Error = upload.Error, Message = upload.Message, Data = new MemeDTO() });
                    }
                    else
                    {
                        _logger.LogInformation("Uploading");
                        await GetTags(item.Tags, meme);
                        _context.Memes.Add(meme);
                        memeResponse.Add(new Response<MemeDTO>() { Data= _mapper.Map<MemeDTO>(meme) , IsSuccess=true, Message="Upload Successful"});
                    }

                    

                }
                
                await _context.SaveChangesAsync();
                _logger.LogInformation("Upload Success");
                return Ok(memeResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine(ex.Message);
                var memeResponse = new List<Response<MemeDTO>>
                {
                    new Response<MemeDTO>() { Error = ex.Message }
                };
                return BadRequest(memeResponse);
            }
            
           
        }

        // DELETE: api/Memes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteMeme(int id)
        { Response<bool> response = new Response<bool>() { Data = false };
            try
            {
                if (_context.Memes == null)
                {
                    return NotFound(response);
                }
                var meme = await _context.Memes.FindAsync(id);
                if (meme == null)
                {
                    return NotFound(response);
                }
                
                    if (meme.UserName != User.Identity.Name)
                    {
                        return Unauthorized(response);
                    }
                
                _context.Memes.Remove(meme);
                await _context.SaveChangesAsync();
                response = new Response<bool>() { Data = true, IsSuccess = true, Message = $"Meme {id} Deleted" };
                _logger.LogInformation($"Username {User.Identity.Name} deleted memeid {id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Username {User.Identity.Name} tried deleting memeid {id}");
                response.Error = ex.Message;
                return BadRequest(response);
            }
           
          
        }
        private async Task GetTags(List<string> tags , Meme meme)
        {
            foreach (var item in tags)
            {
                TagMeme tag = await _context.TagMeme.FirstAsync(p => p.Name == item);
                if (tag != null)
                {
                    tag.Memes.Add(meme);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    tag = new TagMeme { Name = item };
                    tag.Memes.Add(meme);
                    _context.TagMeme.Add(tag);
                   await _context.SaveChangesAsync();
                }
            }
        }
        private bool MemeExists(int id)
        {
            return (_context.Memes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
