using Microsoft.AspNetCore.Mvc;
using Video_Catalogue.Services;

namespace CodingChallenge___Video_Upload___Catalogue.Controllers
{
    public class VideoController : Controller
    {

        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> videoFiles)
        {
            var validationResult = _videoService.ValidateInputVideos(videoFiles);
            if (validationResult.status != 200)
                return StatusCode(validationResult.status, validationResult.Message);

            var saveResult = await _videoService.SaveVideoAsync(videoFiles);
            return StatusCode(saveResult.status, saveResult.Message);
        }


    }
}
