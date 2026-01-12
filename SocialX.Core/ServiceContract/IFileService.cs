using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IFileService
    {
     
        Task<string> UploadFileAsync(
            IFormFile file,
            string folder = "uploads",
            CancellationToken cancellationToken = default);

    
        Task<List<string>> UploadFilesAsync(
            List<IFormFile> files,
            string folder = "uploads",
            CancellationToken cancellationToken = default);

        Task<bool> DeleteFileAsync(
            string fileUrl,
            CancellationToken cancellationToken = default);

        
        Task<bool> DeleteFilesAsync(
            List<string> fileUrls,
            CancellationToken cancellationToken = default);

       
        Task<bool> FileExistsAsync(string fileUrl);

      
        Task<long> GetFileSizeAsync(string fileUrl);

       
        bool ValidateFile(IFormFile file, long maxSizeInBytes, string[] allowedExtensions);
    }
}
