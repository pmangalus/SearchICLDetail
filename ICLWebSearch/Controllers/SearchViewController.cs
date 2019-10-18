using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICLWebSearch.Controllers
{
    public class SearchViewController : Controller
    {
        // GET: SearchView
        public ActionResult Index()
        {
            return View();
        }
    }
}