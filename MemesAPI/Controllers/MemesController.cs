using System;
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

        public MemesController(AppDBContext context, ILogger<MemesController> logger, IMapper mapper, UserManager<MemeUser> userManager, IFileRepository fileRepository)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            this.fileRepository = fileRepository;
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
                if (_context.Memes == null)
                {
                    return NotFound(response);
                }
                var memes = await _context.Memes.Include(l => l.Likes).Include(t => t.Tags).OrderByDescending(m => m.Date).Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize).Take(memeParameters.PageSize).ToListAsync();
                
                foreach (var meme in memes)
                {
                    var dto = _mapper.Map<MemeDTO>(meme);
                    if (meme.Tags.Count > 0)
                    {
                        foreach (var item in meme.Tags)
                        {
                            dto.Tags.Add(item.Name);
                        }
                    }

                    dto.likeCount = meme.Likes.Count();
                    if (User.Identity.IsAuthenticated)
                        dto.like = meme.Likes.Any(z => z.UserName == User.Identity.Name);

                    var responseDTO = new Response<MemeDTO>() { IsSuccess = true, Data = dto, Message = "Found" };
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
                var meme = await _context.Memes.Include(l => l.Likes).Include(t => t.Tags).FirstAsync(m=>m.Id==id);

                if (meme == null)
                {
                    return NotFound(response);
                }
                var dto = _mapper.Map<MemeDTO>(meme);
                if (meme.Tags.Count > 0)
                {
                    foreach (var item in meme.Tags)
                    {
                        dto.Tags.Add(item.Name);
                    }
                }
                
                dto.likeCount = meme.Likes.Count();
                if (User.Identity.IsAuthenticated)
                    dto.like = meme.Likes.Any(z => z.UserName == User.Identity.Name);
                response.IsSuccess = true;
                response.Data= dto;
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
                    Response<string> upload= new Response<string>();
                    var meme = _mapper.Map<Meme>(item);
                    meme.UserName = User.Identity.Name;
                    meme.UserId = User.Claims.First(c => c.Type == "uid").Value;
                    meme.Date = DateTime.UtcNow;
                    if (item.IsFile)
                    {
                         upload = await fileRepository.UploadFile(item.Imgfile);
                        if (upload.IsSuccess)
                        {
                            meme.URLIMG = upload.Data;
                                  meme.Format = item.Imgfile.format;
                        }
                    }
                    else
                    {
                        meme.URLIMG = item.URLIMG;
                    }
                    if (meme.URLIMG == "" || !upload.IsSuccess)
                    {
                        memeResponse.Add(new Response<MemeDTO> { IsSuccess = false, Error = upload.Error, Message = upload.Message, Data = new MemeDTO() });
                    }
                    else
                    {
                        await GetTags(item.Tags, meme);
                        _context.Memes.Add(meme);
                        memeResponse.Add(new Response<MemeDTO>() { Data= _mapper.Map<MemeDTO>(meme) , IsSuccess=true, Message="Upload Successful"});
                    }

                    

                }
                await _context.SaveChangesAsync();
                return Ok(memeResponse);
            }
            catch (Exception ex)
            {
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
        {
            Response<bool> response = new Response<bool>() { Data = false };
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
            response =new Response<bool>(){ Data = true,IsSuccess=true,Message=$"Meme {id} Deleted" };
            return Ok(response);
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
