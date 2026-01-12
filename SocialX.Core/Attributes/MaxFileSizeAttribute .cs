using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace SocialX.Core.Attributes
{ 

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public MaxFileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(
                        $"File {file.FileName} exceeds maximum size of {_maxFileSize / 1024 / 1024}MB");
                }
            }
            else if (value is List<IFormFile> files)
            {
                foreach (var f in files)
                {
                    if (f.Length > _maxFileSize)
                    {
                        return new ValidationResult(
                            $"File {f.FileName} exceeds maximum size of {_maxFileSize / 1024 / 1024}MB");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }


    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(
                        $"File {file.FileName} has invalid extension. Allowed: {string.Join(", ", _extensions)}");
                }
            }
            else if (value is List<IFormFile> files)
            {
                foreach (var f in files)
                {
                    var extension = Path.GetExtension(f.FileName).ToLower();
                    if (!_extensions.Contains(extension))
                    {
                        return new ValidationResult(
                            $"File {f.FileName} has invalid extension. Allowed: {string.Join(", ", _extensions)}");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

    public class MaxFileCountAttribute : ValidationAttribute
    {
        private readonly int _maxCount;

        public MaxFileCountAttribute(int maxCount)
        {
            _maxCount = maxCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is List<IFormFile> files)
            {
                if (files.Count > _maxCount)
                {
                    return new ValidationResult($"Maximum {_maxCount} files allowed");
                }
            }

            return ValidationResult.Success;
        }
    }
}