namespace WebDesk.Controllers
{
    /// <summary>
    /// Test controller
    /// </summary>
    [AllowAnonymous]
    public class DemoContainersController: Controller
    {
        /* actions */
        [Route("/demo/DropDownBox")]
        public IActionResult DropDownBox()
        {
            return View();
        }
        [Route("/demo/VirtualScroller")]
        public IActionResult VirtualScroller()
        {
            return View();
        }
        [Route("/demo/Splitter")]
        public IActionResult Splitter()
        {
            return View();
        }
        [Route("/demo/GroupBox")]
        public IActionResult GroupBox()
        {
            return View();
        }
        [Route("/demo/Accordion")]
        public IActionResult Accordion()
        {
            return View();
        }
        [Route("/demo/TabControl")]
        public IActionResult TabControl()
        {
            return View();
        }
        [Route("/demo/PanelList")]
        public IActionResult PanelList()
        {
            return View();
        }
        [Route("/demo/ImageSlider")]
        public IActionResult ImageSlider()
        {
            return View();
        }
        [Route("/demo/IFrame")]
        public IActionResult IFrame()
        {
            return View();
        }

        [Route("/demo/ElementResizeDetector")]
        public IActionResult ElementResizeDetector()
        {
            return View();
        }
        [Route("/demo/ElementSizeMode")]
        public IActionResult ElementSizeMode()
        {
            return View();
        }

        
    }
}
