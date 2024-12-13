namespace WebDesk.Controllers
{

    /// <summary>
    /// Test controller
    /// </summary>
    [AllowAnonymous]
    public class DemoDataController : Controller
    {
        /* actions */
        [Route("/demo/SimpleBinding")]
        public IActionResult SimpleBinding()
        {
            return View();
        }
        [Route("/demo/ListBinding")]
        public IActionResult ListBinding()
        {
            return View();
        }
        [Route("/demo/MasterDetail")]
        public IActionResult MasterDetail()
        {
            return View();
        }
        [Route("/demo/BrokerView")]
        public IActionResult BrokerView()
        {
            return View();
        }

        [Route("/demo/Locator")]
        public IActionResult Locator()
        {
            return View();
        }
    }
}
