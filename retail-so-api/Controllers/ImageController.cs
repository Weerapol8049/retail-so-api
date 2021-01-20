using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using retail_so_api.Models;
using Microsoft.AspNetCore.Hosting;

namespace retail_so_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageDBContext _context;
        private readonly IHostingEnvironment _hostEnvironment;

        public ImageController(ImageDBContext context, IHostingEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Image
        [HttpGet]
        public IEnumerable<Image> GetImages()
        {
            return _context.Images;
        }

        [HttpGet("{refRecId}")]
        public IEnumerable<Image> GetImages(Int64 refRecId)
        {
            var refRecIdParm = new SqlParameter("@refRecId", refRecId);

            return _context.Images.FromSql(@"SELECT [RECID]
                                                  ,[IMAGE]
                                                  ,[NAME]
                                                  ,[SALESSODAILY]
                                                  ,[CREATEDATETIME]
                                              FROM [dbo].[STMSALESSODAILYIMAGES]
                                              WHERE SALESSODAILY = @refRecId
                                              ORDER BY [NAME]", refRecIdParm).ToList();
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage([FromRoute] long id, [FromBody] Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.RecId)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/Image
        [HttpPost]
        public async Task<IActionResult> PostImage([FromForm] Image image)
        {
            image.Name = await SaveImage(image.ImageFile);
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Image/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return Ok(image);
        }

        private bool ImageExists(long id)
        {
            return _context.Images.Any(e => e.RecId == id);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }
}