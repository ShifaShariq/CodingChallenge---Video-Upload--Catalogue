using Video_Catalogue.Models;
namespace Video_Catalogue.Services
{
    public interface IVideoService
    {
        public (int status, string Message) ValidateInputVideos(List<IFormFile> videos);
        public Task<(int status, string Message)> SaveVideoAsync(List<IFormFile> videos);
        public List<VideoDetailModel> GetAllVideos();
    }
}
