
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SocialX.Core.ServiceContract;
using Microsoft.AspNetCore.Hosting;

namespace SocialX.Application.FileService
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(
            IFormFile file,
            string folder = "uploads",
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null");
                }


                var uploadsFolder = Path.Combine(_environment.WebRootPath, folder);


                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Created directory: {Directory}", uploadsFolder);
                }


                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                }

                _logger.LogInformation("File uploaded successfully: {FileName}", uniqueFileName);

                return $"/{folder}/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {FileName}", file?.FileName);
                throw;
            }
        }

        public async Task<List<string>> UploadFilesAsync(
            List<IFormFile> files,
            string folder = "uploads",
            CancellationToken cancellationToken = default)
        {
            var uploadedUrls = new List<string>();

            if (files == null || !files.Any())
            {
                return uploadedUrls;
            }

            foreach (var file in files)
            {
                try
                {
                    var url = await UploadFileAsync(file, folder, cancellationToken);
                    uploadedUrls.Add(url);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file: {FileName}", file.FileName);

                }
            }

            return uploadedUrls;
        }

        public Task<bool> DeleteFileAsync(
            string fileUrl,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileUrl))
                {
                    return Task.FromResult(false);
                }


                var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                    return Task.FromResult(true);
                }

                _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
                return Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteFilesAsync(
            List<string> fileUrls,
            CancellationToken cancellationToken = default)
        {
            if (fileUrls == null || !fileUrls.Any())
            {
                return false;
            }

            var allDeleted = true;

            foreach (var fileUrl in fileUrls)
            {
                var deleted = await DeleteFileAsync(fileUrl, cancellationToken);
                if (!deleted)
                {
                    allDeleted = false;
                }
            }

            return allDeleted;
        }

        public Task<bool> FileExistsAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileUrl))
                {
                    return Task.FromResult(false);
                }

                var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                return Task.FromResult(File.Exists(filePath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking file existence: {FileUrl}", fileUrl);
                return Task.FromResult(false);
            }
        }

        public Task<long> GetFileSizeAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileUrl))
                {
                    return Task.FromResult(0L);
                }

                var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    return Task.FromResult(fileInfo.Length);
                }

                return Task.FromResult(0L);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file size: {FileUrl}", fileUrl);
                return Task.FromResult(0L);
            }
        }

        public bool ValidateFile(IFormFile file, long maxSizeInBytes, string[] allowedExtensions)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File validation failed: File is null or empty");
                return false;
            }

            if (file.Length > maxSizeInBytes)
            {
                _logger.LogWarning(
                    "File validation failed: File size {Size} exceeds maximum {MaxSize}",
                    file.Length, maxSizeInBytes);
                return false;
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                _logger.LogWarning(
                    "File validation failed: Extension {Extension} not allowed",
                    fileExtension);
                return false;
            }

            return true; 
        }
    }

    }

