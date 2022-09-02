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
        public async Task<ActionResult<IEnumerable<Meme>>> GetMemes([FromQuery] MemeParameters memeParameters)
        {
          if (_context.Memes == null)
          {
              return NotFound();
          }
            return await _context.Memes.Skip((memeParameters.PageNumber-1)*memeParameters.PageSize).Take(memeParameters.PageSize).ToListAsync();
        }

        // GET: api/Memes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meme>> GetMeme(int id)
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

            return meme;
        }

        // PUT: api/Memes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize()]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeme(int id, Meme meme)
        {
            if (id != meme.Id && _contextAccessor.HttpContext.User.Identity.Name==meme.UserName);
            {
                return BadRequest();
            }

            _context.Entry(meme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Memes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Meme>> PostMeme(Meme meme)
        {
          if (_context.Memes == null)
          {
              return Problem("Entity set 'AppDBContext.Memes'  is null.");
          }
            meme.UserName = _contextAccessor.HttpContext.User.Identity.Name;
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
