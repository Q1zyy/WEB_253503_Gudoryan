using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WEB_253503_Gudoryan.Application.Extensions
{
	public static class HttpRequestExtensions
	{
		public static bool IsAjaxRequest(this HttpRequest request)
		{
			return request.Headers["x-requested-with"].ToString().ToLower().Equals("xmlhttprequest");
		}
	}
}
