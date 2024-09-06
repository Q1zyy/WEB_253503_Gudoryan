namespace WEB_253503_Gudoryan.Views.Shared.Components.Cart
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public class Cart : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

