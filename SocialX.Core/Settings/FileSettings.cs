namespace SocialX.Core.Settings
{
    public class FileSettings
    {
        public long MaxImageSize { get; set; } = 5 * 1024 * 1024; // 5MB
        public long MaxVideoSize { get; set; } = 50 * 1024 * 1024; // 50MB
        public long MaxDocumentSize { get; set; } = 10 * 1024 * 1024; // 10MB

        public string[] AllowedImageExtensions { get; set; } = new[]
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"
        };

        public string[] AllowedVideoExtensions { get; set; } = new[]
        {
            ".mp4", ".mov", ".avi", ".wmv", ".flv", ".webm"
        };

        public string[] AllowedDocumentExtensions { get; set; } = new[]
        {
            ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx"
        };

        public int MaxFilesPerUpload { get; set; } = 4;

        public string UploadFolder { get; set; } = "uploads";
        public string TweetsFolder { get; set; } = "uploads/tweets";
        public string CommentsFolder { get; set; } = "uploads/comments";
        public string ProfilesFolder { get; set; } = "uploads/profiles";
    }
}