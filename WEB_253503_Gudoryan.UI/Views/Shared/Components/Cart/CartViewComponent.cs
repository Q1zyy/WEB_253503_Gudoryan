namespace WEB_253503_Gudoryan.UI.Views.Shared.Components.Cart
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;
    using WEB_253503_Gudoryan.Application.Extensions;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
	using WEB_253503_Gudoryan.Domain.Entities;

	public class CartViewComponent : ViewComponent
    {

        private readonly Cart _cart;

        public CartViewComponent(Cart cart) 
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}

