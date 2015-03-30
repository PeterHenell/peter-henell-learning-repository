using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace peterhenell.se.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";
			
			return View ();
		}

		public JsonResult SomeActionMethod() 
		{ 
			int inputId;
			if (int.TryParse (Request ["id"], out inputId)) {
				// do lookup
				return Json (new {foo = "bar", baz = "Blech", id = inputId}, JsonRequestBehavior.AllowGet);
			} else {
				throw new FormatException ("id");
			}
		}

		public JsonResult PostMethod(){
			return Json (new {coolness = "uber", when = DateTime.Now.ToString ()});
		}
	}
}

