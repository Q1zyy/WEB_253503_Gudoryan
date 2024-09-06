using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Gudoryan.Models;

namespace WEB_253503_Gudoryan.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			ViewData["Title"] = "Лабораторная работа 2";
			List<ListDemo> listDemo = new List<ListDemo>()
			{
				new ListDemo {Id = 1, Name = "Item1"},
				new ListDemo {Id = 2, Name = "Item2"},
				new ListDemo {Id = 3, Name = "Item3"},
				new ListDemo {Id = 4, Name = "Item4"},
			};
			SelectList selectListItems = new SelectList(listDemo, "Id", "Name");
			return View(selectListItems);
		}
	}
}
