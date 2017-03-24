namespace Sitecore.ControllerLayout.Web.Controllers
{
    using System.Web.Mvc;
    using Models;

    public class ControllerLayoutController : Controller
    {
        private const string ControllerLayoutActionView = "~/Views/Controller Layout/Layouts/Default.cshtml";

        public ActionResult ControllerLayoutAction()
        {
            return View(ControllerLayoutActionView, new PageModel { Title = Context.Item["Title"], BodyClass = "regular" });
        }
    }
}