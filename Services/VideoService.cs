using Video_Catalogue.Models;

namespace Video_Catalogue.Services
{
    public class VideoService : IVideoService
    {
        private readonly string _mediaPath;
        public VideoService() {
            _mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media");
            if (!Directory.Exists(_mediaPath))
                Directory.CreateDirectory(_mediaPath);
        }

        public (int status, string Message) ValidateInputVideos(List<IFormFile> videoFiles)
        {
            if (videoFiles == null || !videoFiles.Any())
                return (400, "No files could be uploaded. Please try again."); // 400 Bad Request

            try
            {
                foreach (var file in videoFiles)
                {
                    if (file == null || file.Length == 0)
                        return (422, "One or more files are empty. Please try uploading some other file."); // 422 Unprocessable Entity

                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (extension != ".mp4")
                        return (415, "Unsupported file extension. Please try uploading .mp4 files only"); // 415 Unsupported Media Type

                    if (file.ContentType != "video/mp4")
                        return (406, "Invalid MIME type. Must be video/mp4."); // 406 Not Acceptable

                    if (file.Length > 200 * 1024 * 1024)
                        return (413, "File size exceeds 200 MB limit."); // 413 Payload Too Large
                   
                }

                return (200, "Validation successful.");

            }
            catch (Exception ex)
            {
                return (500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<(int status, string Message)> SaveVideoAsync(List<IFormFile> videoFiles)
        {
            try
            {
                foreach (var file in videoFiles)
                {
                    var filePath = Path.Combine(_mediaPath, Path.GetFileName(file.FileName));
                    if (Directory.Exists(filePath))
                    {
                        Directory.Delete(filePath);                                //Overwrite existing file
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return (200, "Upload successful.");
            }
            catch (Exception ex)
            {
                return (500, $"Internal server error while uploading files: {ex.Message}");
            }
        }

        public List<VideoDetailModel> GetAllVideos()
        {
            var videosList = Directory.GetFiles(_mediaPath)?.Select(item =>
                new VideoDetailModel
                {
                    FileName = Path.GetFileName(item),
                    FileSize = new FileInfo(item).Length
                }).ToList();

            return videosList ?? new List<VideoDetailModel>();

        }

    }
}
