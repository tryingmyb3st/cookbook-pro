using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace CookbookTests.Controllers;

public class FileControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public FileControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Upload_WithValidImage_ReturnsFileName()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("file@test.com");

        var content = new MultipartFormDataContent();
        var imageBytes = new byte[] { 1, 2, 3, 4, 5 };
        var byteContent = new ByteArrayContent(imageBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(byteContent, "file", "test.jpg");

        var response = await client.PostAsync("/cookbook/File/Upload", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fileName = await response.Content.ReadAsStringAsync();
        fileName.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Upload_WithInvalidExtension_ReturnsBadRequest()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();

        var content = new MultipartFormDataContent();
        var bytes = new byte[] { 1, 2, 3 };
        var byteContent = new ByteArrayContent(bytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(byteContent, "file", "test.txt");

        var response = await client.PostAsync("/cookbook/File/Upload", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Download_ExistingFile_ReturnsOk()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("download@test.com");
        var fileName = "existing.jpg";
        var response = await client.GetAsync($"/cookbook/File/Download?fileName={fileName}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response.Content.Headers.ContentType!.MediaType.Should().Be("image/jpeg");

        var bytes = await response.Content.ReadAsByteArrayAsync();
        bytes.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Delete_ExistingFile_ReturnsOk()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("delete@test.com");
        var fileName = "to-delete.jpg";
        var response = await client.DeleteAsync($"/cookbook/File/Delete?fileName={fileName}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json!["message"].Should().Be($"Изображение {fileName} удалено");
    }

}
