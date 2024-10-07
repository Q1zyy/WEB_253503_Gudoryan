using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_Gudoryan.Application.Services.GameService;
using WEB_253503_Gudoryan.Domain.Entities;
using WEB_253503_Gudoryan.Application.Extensions;


namespace WEB_253503_Gudoryan.UI.Controllers
{
	public class CartController : Controller
	{

		private readonly IGameService _gameService;
		private readonly Cart _cart;

		public CartController(IGameService gameService, Cart cart)
		{
			_gameService = gameService;
			_cart = cart;
		}

		public IActionResult Index() 
		{
			return View(_cart);
		}

		[Authorize]
		public async Task<IActionResult> Add(int id, string returnUrl)
		{
			var data = await _gameService.GetGameByIdAsync(id);
			if (data.Successful)
			{
				_cart.AddToCart(data.Data);
			}
			return Redirect(returnUrl);
		}	
		
		[Authorize]
		public IActionResult Decrease(int id, string returnUrl)
		{
			_cart.DecreaseFromCart(id);
			return Redirect(returnUrl);
		}	
		
		
		[Authorize]
		public IActionResult Remove(int id, string returnUrl)
		{
			_cart.RemoveFromCart(id);
			return Redirect(returnUrl);
		}

	}
}
