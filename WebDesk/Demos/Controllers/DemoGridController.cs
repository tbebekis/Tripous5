namespace WebDesk.Controllers
{
    [AllowAnonymous]
    public class DemoGridController : Controller
    {

        /* actions */
        [Route("/demo/Grid")]
        public IActionResult Grid()
        {
            return View();
        }
        [Route("/demo/Grid2")]
        public IActionResult Grid2()
        {
            return View();
        }
        [Route("/demo/Grid3")]
        public IActionResult Grid3()
        {
            return View();
        }
    }
}
