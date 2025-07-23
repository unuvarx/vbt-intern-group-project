using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStoreAPI.Data;
using PetStoreAPI.Models;
using System.Linq;

namespace PetStoreAPI.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PostController> _logger;

        public PostController(AppDbContext db, ILogger<PostController> logger)
        {
            _db = db;
            _logger = logger;
        }

        //  Tüm postları listeleyen endpoint
        // GET api/post/get-all
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(List<Post>), StatusCodes.Status200OK)]  // Başarılı yanıt, Post listesi döner
        public async Task<IActionResult> GetAllPosts()
        {
            _logger.LogInformation("GET api/post/get-all çalıştı");

            var posts = await _db.Posts
                .Include(p => p.ApplicationUser)
                .Include(p => p.AdoptionRequests)
                .OrderByDescending(p => p.PostId)
                .ToListAsync();

            return Ok(posts);
        }

        // ID ile post getirme endpointi
        // GET api/post/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)] // Başarılı yanıt: tek bir Post döner
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)] // ID bulunmazsa sadece string mesaj döner
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)] // Geçersiz ID girilirse string mesaj döner
        public async Task<IActionResult> GetPostById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Geçersiz Post ID isteği: {Id}", id);
                return BadRequest("Post ID sıfırdan büyük olmalıdır.");  // string döner
            }

            _logger.LogInformation("GET api/post/{Id} çalıştı", id);

            var post = await _db.Posts
                .Include(p => p.ApplicationUser)
                .Include(p => p.AdoptionRequests)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                _logger.LogWarning("Post ID {Id} bulunamadı.", id);
                return NotFound($"ID'si {id} olan gönderi bulunamadı."); // string döner
            }

            return Ok(post); // Post objesi döner
        }
    }
}
