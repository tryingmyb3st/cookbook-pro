using CookbookFileStorage;
using CookbookWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]/[action]")]
[Authorize]
public class FileController(
    IFileService fileService,
    ILogger<FileController> logger): ControllerBase
{
    private readonly IFileService _fileService = fileService;
    private readonly ILogger<FileController> _logger = logger;

    private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    [HttpPost]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Файл не выбран" });

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(fileExtension))
                return BadRequest(new
                {
                    message = "Недопустимый формат файла. Разрешены: jpg, jpeg, png, gif, webp"
                });

            if (file.Length > 20 * 1024 * 1024)
                return BadRequest(new { message = "Файл слишком большой. Максимальный размер: 20MB" });

            var fileName = await _fileService.UploadFileAsync(file, $"{Guid.NewGuid()}{fileExtension}");
            var imageUrl = _fileService.GetPublicUrl(fileName);

            var response = new FileUploadResponse
            {
                FileName = fileName
            };

            return Ok(fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Ошибка при загрузке файла: {ex.Message}" });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download([FromQuery] string fileName)
    {
        try
        {
            var fileStream = await _fileService.DownloadFileAsync(fileName);

            if (fileStream == null)
            {
                return NotFound(new { message = "Файл не найден" });
            }

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            return File(fileStream, contentType, fileName);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "Файл не найден: {FileName}", fileName);
            return NotFound(new { message = "Файл не найден" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при скачивании файла {FileName}", fileName);
            return StatusCode(500, new { message = $"Ошибка при скачивании файла: {ex.Message}" });
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromQuery] string fileName)
    {
        try
        {
            await _fileService.DeleteFileAsync(fileName);
            return Ok(new { message = $"Изображение {fileName} удалено" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении изображения {FileName}", fileName);
            return NotFound(new { message = "Файл не найден" });
        }
    }
}