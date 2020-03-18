namespace OSA.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OSA.Web.ValidationEnum;

    public class BaseController : Controller
    {
        public void SetFlash(FlashMessageType type, string text)
        {
            this.TempData["FlashMessage.Type"] = type;
            this.TempData["FlashMessage.Text"] = text;
        }
    }
}
