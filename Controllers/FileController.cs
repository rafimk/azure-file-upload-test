using Azure.Storage.Blobs;
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
        // string systemFileName = files.FileName;  
        // string blobstorageconnection = _configuration.GetValue<string>("BlobConnectionString");  
        // CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);  
        // CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();  
        // CloudBlobContainer container = blobClient.GetContainerReference(_configuration.GetValue<string>("BlobContainerName"));  
        // CloudBlockBlob blockBlob = container.GetBlockBlobReference(systemFileName);  
        // await using(var data = files.OpenReadStream()) 
        // {
        //     await blockBlob.UploadFromStreamAsync(data);
        // }

        // var localFilePath = "Bugs-1.xlsx";

        string blobstorageconnection = _configuration.GetValue<string>("BlobConnectionString"); 
        string containerName = _configuration.GetValue<string>("BlobContainerName");

        // BlobServiceClient blobServiceClient = new BlobServiceClient(blobstorageconnection);

        // BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobServiceClient.Uri);
        // BlobClient blobClient = containerClient.GetBlobClient("Bugs-1.xlsx");

        // using FileStream uploadFileStream = File.OpenRead(localFilePath);

        // blobClient.Upload(uploadFileStream);
        // uploadFileStream.Close();
        // Create a BlobServiceClient object which will be used to create a container client
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        //Create a unique name for the container
        // string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

        // Create the container and return a container client object
        // BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Create a local file in the ./data/ directory for uploading and downloading
        string localPath = "./data/";
        string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
        string localFilePath = Path.Combine(localPath, fileName);

        // Write text to the file
        await File.WriteAllTextAsync(localFilePath, "Hello, World!");

        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

        // Upload data from the local file
        await blobClient.UploadAsync(localFilePath, true);

        return Ok("File Uploaded Successfully");  
    }   
}
