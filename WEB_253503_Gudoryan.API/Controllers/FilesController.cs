using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;

namespace WEB_253503_Gudoryan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        
        private readonly string _imagePath;
        
        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }
            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            
            using var fileStream = fileInfo.Create();
           
            await file.CopyToAsync(fileStream);
            
            var host = HttpContext.Request.Host;
            
            var fileUrl = $"Https://{host}/Images/{file.FileName}";
            
            return Ok(fileUrl);
        }

        [HttpDelete]
        public IActionResult DeleteFile([FromForm] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be null or empty.");
            }

            var filePath = Path.Combine(_imagePath, fileName);
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return NotFound("File not found.");
            }

            fileInfo.Delete();
            return Ok("File deleted successfully.");
        }
    }
}
