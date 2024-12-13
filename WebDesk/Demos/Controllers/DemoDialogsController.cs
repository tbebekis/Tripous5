namespace WebDesk.Controllers
{

    /// <summary>
    /// A test controller
    /// </summary>
    [AllowAnonymous]
    public class DemoDialogsController: Controller
    {

        /* actions */
        [Route("/demo/Notifications")]
        public IActionResult Notifications()
        {
            return View();
        }
        [Route("/demo/NotificationDialogs")]
        public IActionResult NotificationDialogs()
        {
            return View();
        }
       
        [Route("/demo/FrameBox")]
        public IActionResult FrameBox()
        {
            return View();
        }
        [Route("/demo/ContentWindow")]
        public IActionResult ContentWindow()
        {
            return View();
        }


        [Route("/demo/DataSetBox")]
        public IActionResult DataSetBox()
        {
            return View();
        }
        [Route("/demo/TableBox")]
        public IActionResult TableBox()
        {
            return View();
        }
      
        [Route("/demo/RowBox")]
        public IActionResult RowBox()
        {
            return View();
        }
        [Route("/demo/PickRowsBox")]
        public IActionResult PickRowsBox()
        {
            return View();
        }
        [Route("/demo/PickRowBox")]
        public IActionResult PickRowBox()
        {
            return View();
        }

        [Route("/demo/SqlFilterBox")]
        public IActionResult SqlFilterBox()
        {
            return View();
        }


        [Route("/demo/MiscDialogs")]
        public IActionResult MiscDialogs()
        {
            return View();
        }
        
    }
}