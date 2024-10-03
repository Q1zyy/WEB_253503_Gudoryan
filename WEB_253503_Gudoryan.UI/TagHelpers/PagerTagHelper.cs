using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_253503_Gudoryan.UI.TagHelpers
{
	[HtmlTargetElement(tag: "pager")]
	public class PagerTagHelper : TagHelper
	{

		[HtmlAttributeName("current-page")]
		public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages {  get; set; }

        [HtmlAttributeName("category")]
        public string? Category { get; set; }

        [HtmlAttributeName("admin")]
        public bool? IsAdmin { get; set; }

		private readonly LinkGenerator _linkGenerator;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
		{
			_linkGenerator = linkGenerator;
			_httpContextAccessor = httpContextAccessor;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			int cur = CurrentPage;
			int prev = CurrentPage == 1 ? 1 : CurrentPage - 1;
			int next = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;
			int one = cur - 1, two = cur, three = cur + 1;
			if (prev == cur)
			{
				one = 1;
				two = 2;
				three = 3;
			}
			if (next == cur)
			{
				three = cur;
				two = cur - 1;
				one = cur - 2;
			}

			List<int> pages = new List<int>() { one, two, three };


			output.TagName = "nav"; 
			var content = await output.GetChildContentAsync();

			var ul = new TagBuilder("ul");
			ul.AddCssClass("pagination");
			var Prev = new TagBuilder("li");
			var aPrev = new TagBuilder("a");
			aPrev.AddCssClass("page-link");
			string urlPrev = GenerateUrl(prev);
			aPrev.Attributes["href"] = urlPrev;
			aPrev.InnerHtml.Append("Prev");
			Prev.InnerHtml.AppendHtml(aPrev);
			ul.InnerHtml.AppendHtml(Prev);

			foreach (var i in pages)
			{
				if (i > 0 && i <= TotalPages)
				{
					var li = new TagBuilder("li");
					li.AddCssClass("page-item");
					var a = new TagBuilder("a");
					a.AddCssClass("page-link");

					if (i == CurrentPage)
					{
						li.AddCssClass("active");
					}

					string url = GenerateUrl(i);
					a.Attributes["href"] = url;
					a.InnerHtml.Append(i.ToString());

					li.InnerHtml.AppendHtml(a);
					ul.InnerHtml.AppendHtml(li);
				}
			}


			var Next = new TagBuilder("li");
			var aNext = new TagBuilder("a");
			aNext.AddCssClass("page-link");
			string urlNext = GenerateUrl(next);
			aNext.Attributes["href"] = urlNext;
			aNext.InnerHtml.Append("Next");
			Next.InnerHtml.AppendHtml(aNext);
			ul.InnerHtml.AppendHtml(Next);

			output.Content.AppendHtml(ul);
		}	

		private string GenerateUrl(int pageNumber)
		{
			var context = _httpContextAccessor.HttpContext;
			return _linkGenerator.GetPathByPage(context, null, null, new { pageNo = pageNumber, category = Category });
		}

	}
}
