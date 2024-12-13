namespace WebDesk.Controllers
{
    [AllowAnonymous]
    public class DemoController : Controller
    {
        [Route("/demos")]
        public IActionResult Demos()
        {
            return View();
        }

        [Route("/demo/DomElements")]
        public IActionResult DomElements()
        {
            return View();
        }
        [Route("/demo/TabIndexAndFocus")]
        public IActionResult TabIndexAndFocus()
        {
            return View();
        }
        [Route("/demo/EmptyTest")]
        public IActionResult EmptyTest()
        {
            return View();
        }


    }
}
