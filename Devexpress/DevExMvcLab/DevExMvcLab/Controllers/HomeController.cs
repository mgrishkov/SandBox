using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExMvcLab.Models;

namespace DevExMvcLab.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GridPartial()
        {
            var model = new List<TestModel>();
            model.Add(new TestModel() { Field01 = "Field01-01", Field02 = "Field02-01" });
            model.Add(new TestModel() { Field01 = "Field01-02", Field02 = "Field02-02" });
            model.Add(new TestModel() { Field01 = "Field01-03", Field02 = "Field02-03" });
            model.Add(new TestModel() { Field01 = "Field01-04", Field02 = "Field02-04" });
            model.Add(new TestModel() { Field01 = "Field01-05", Field02 = "Field02-05" });

            return PartialView("GridPartial", model);
        }
    }
}
