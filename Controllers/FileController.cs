using WindowsAzure.Storage;
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

    [HttpPost(nameof(UploadFile))]  
    public async Task<IActionResult> UploadFile(IFormFile files) 
    {  
        string systemFileName = files.FileName;  
        string blobstorageconnection = _configuration.GetValue<string>("BlobConnectionString");  
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
        CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();  
        CloudBlobContainer container = blobClient.GetContainerReference(_configuration.GetValue<string>("BlobContainerName"));  
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);  
        await using(var data = files.OpenReadStream()) 
        {
            await blockBlob.UploadFromStreamAsync(data);
        }
        return Ok("File Uploaded Successfully");  
    }   
}
