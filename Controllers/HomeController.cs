using Microsoft.AspNetCore.Mvc;
using Video_Catalogue.Services;

namespace Video_Catalogue.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoService _videoService;

        public HomeController( IVideoService videoService) 
        { 

            _videoService = videoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult CataloguePartial()
        {
            var videosList = _videoService.GetAllVideos();
            return PartialView("_Catalogue", videosList);
        }

        public PartialViewResult UploadPartial()
        {
            return PartialView("_Upload");
        }
    }
}
