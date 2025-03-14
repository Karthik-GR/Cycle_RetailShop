using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("api/images")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ImageController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Upload image
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var image = new ImageModel
                {
                    FileName = file.FileName,
                    ImageData = memoryStream.ToArray()
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Image uploaded successfully" });
        }
        catch (Exception ex)
        {
            // Log the exception and return an internal server error
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }


    // Get all images
    [HttpGet]
    public async Task<IActionResult> GetImages()
    {
        var images = await _context.Images.ToListAsync();
        return Ok(images.Select(i => new { i.Id, i.FileName }));
    }

    // Get image by Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null)
        {
            return NotFound();
        }

        return File(image.ImageData, "image/jpeg");
    }
}
