namespace WebDesk.Controllers
{
    /// <summary>
    /// A test controller
    /// </summary>
    [AllowAnonymous]
    public class DemoMenusController : Controller
    {
        /* actions */
        [Route("/demo/Button")]
        public IActionResult Button()
        {
            return View();
        }
        [Route("/demo/ToolBar")]
        public IActionResult ToolBar()
        {
            return View();
        }
        [Route("/demo/ItemBar")]
        public IActionResult ItemBar()
        {
            return View();
        }

        [Route("/demo/Menu")]
        public IActionResult Menu()
        {
            return View();
        }
        [Route("/demo/ContextMenu")]
        public IActionResult ContextMenu()
        {
            return View();
        }

        [Route("/demo/SiteMenu")]
        public IActionResult SiteMenu()
        {
            return View();
        }

    }
}
