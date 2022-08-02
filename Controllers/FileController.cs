using Azure.Storage.Blobs;
using azure_file_upload_test.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace azure_file_upload_test.Controllers;

[Route("api/[controller]")]  
[ApiController]  
public class FileController: ControllerBase 
{
    private readonly IConfiguration _configuration;  

    public FileController(IConfiguration configuration) 
    {  
        _configuration = configuration;  
    }

    [HttpPost]  
    public async Task<IActionResult> UploadFile(IFormFile files) 
    {  
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=kmccfileupload;AccountKey=h+PFVsQg8A6A9S43/lDDABLNO6GyzGTmGRgH6op5KHwXo85jyBrD7XivcCtWvZvZJHNFIp84my43+AStWH/JCw==;EndpointSuffix=core.windows.net";
        string containerName = "images";
            
        // Create a BlobServiceClient object which will be used to create a container client
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        string fileName =  files.FileName;
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        if (files.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                files.CopyTo(ms);
                ms.Position = 0;
                await blobClient.UploadAsync(ms, true);
            }
        }

        return Ok("File Uploaded Successfully");  
    }   
}
