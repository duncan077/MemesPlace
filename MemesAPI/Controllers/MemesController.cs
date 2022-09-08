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
        private readonly IHttpContextAccessor _contextAccessor;

        public MemesController(AppDBContext context, ILogger<MemesController> logger, IMapper mapper, UserManager<MemeUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        // GET: api/Memes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemeDTO>>> GetMemes([FromQuery] MemeParameters memeParameters)
        {
          if (_context.Memes == null)
          {
              return NotFound();
          }
            var memes = await _context.Memes.OrderBy(m => m.Date).Where(z=>z.Name.Contains(memeParameters.name)).Skip((memeParameters.PageNumber - 1) * memeParameters.PageSize).Take(memeParameters.PageSize).ToListAsync();
          List<MemeDTO> result = new List<MemeDTO>();
            foreach (var meme in memes)
            {
                var dto = new MemeDTO();
                dto.Desc = meme.Description;
                dto.imgURL = meme.URLIMG;
                dto.Name = meme.Name;
                dto.UserName = meme.UserName;
                dto.Date = meme.Date;
                dto.likeCount = meme.Likes.Count();
                if(User.Identity.IsAuthenticated)
                dto.like = meme.Likes.Any(z => z.UserName == User.Identity.Name);
            
                result.Add(dto);
            }
          
            
            return Ok(result);
        }

        // GET: api/Memes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemeDTO>> GetMeme(int id)
        {
          if (_context.Memes == null)
          {
              return NotFound();
          }
            var meme = await _context.Memes.FindAsync(id);

            if (meme == null)
            {
                return NotFound();
            }
            MemeDTO dto = new MemeDTO();
                dto.Desc = meme.Description;
                dto.imgURL = meme.URLIMG;
                dto.Name = meme.Name;
                dto.UserName = meme.UserName;
                dto.Date = meme.Date;
                dto.likeCount = meme.Likes.Count;
            if(User.Identity.IsAuthenticated)
                dto.like = meme.Likes.Any(z => z.UserName == User.Identity.Name);
            else
            dto.like = false;
            return dto;
        }

        [HttpPost("like/{id}")]
        [Authorize]
        public async Task<ActionResult> LikeMeme(int id)
        {
            
            if (_context.Memes == null)
            {
                return NotFound();
            }
            var meme = await _context.Memes.FindAsync(id);
            MemeUser user = (MemeUser)_context.Users.Where(x=>x.UserName.Equals(_contextAccessor.HttpContext.User.Identity.Name)).First();
            
            if (meme == null)
            {
                return NotFound();
            }
            var like = meme.Likes.First(z => z.UserName == User.Identity.Name);
            if (like!=null)
            {

                _context.MemeLike.Remove(like);
                _context.SaveChanges();
            }
            else
            {
                 like = new MemeLike();
                like.UserName = User.Identity.Name;
                like.Meme = id;
                like.DateTime = DateTime.UtcNow;
                 _context.MemeLike.Add(like);
                _context.SaveChanges();
                
            }



            return Accepted();
        }

        // POST: api/Memes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Meme>> PostMeme(MemeAddDTO memeDTO)
        {
            var meme = new Meme();
          if (_context.Memes == null)
          {
              return Problem("Entity set 'AppDBContext.Memes'  is null.");
          }
            meme.Name = memeDTO.Name;
            meme.Description=memeDTO.Description;
            meme.URLIMG = memeDTO.URLIMG;
            meme.UserName = _contextAccessor.HttpContext.User.Identity.Name;
            meme.Date = new DateTime().Date;
            _context.Memes.Add(meme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeme", new { id = meme.Id }, meme);
        }

        // DELETE: api/Memes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeme(int id)
        {
            if (_context.Memes == null)
            {
                return NotFound();
            }
            var meme = await _context.Memes.FindAsync(id);
            if (meme == null)
            {
                return NotFound();
            }
            if (meme.UserName != _contextAccessor.HttpContext.User.Identity.Name)
            { 
            return Unauthorized();
            }
            _context.Memes.Remove(meme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemeExists(int id)
        {
            return (_context.Memes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
