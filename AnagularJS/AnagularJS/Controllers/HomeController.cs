using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnagularJS.Models;

namespace AnagularJS.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDriversList()
        {
            var result = new List<Driver>();

            var driver1 = new Driver() { ID = 1, FirstName = "максим", LastName="гришков", BirthDate = new DateTime(1986,9,27) };
            driver1.Vehicle = new Vehicle() 
            { 
                Brand = "Honda", 
                Model = "Insight", 
                RegistrationTag = "М722НУ 178RUS",
                ImageUrl = "http://www.hondacar-club.ru/uploads/posts/2013-09/thumbs/1378423242_2009_honda_insight_hybrid.jpg"
            };
            result.Add(driver1);

            var driver2 = new Driver() { ID = 2, FirstName = "сергей", LastName="осташев", BirthDate = new DateTime(1983,2,11) };
            driver2.Vehicle = new Vehicle() 
            { 
                Brand = "Renault", 
                Model="Clio RS" ,
                ImageUrl = "http://a2goos.com/data_images/galleryes/renault-clio-rs/renault-clio-rs-04.jpg"
            };
            result.Add(driver2);

            var driver3 = new Driver() { ID = 3, FirstName = "евгений", LastName="ненашев" };
            driver3.Vehicle = new Vehicle() 
            { 
                Brand = "Volvo", 
                Model="S80",
                ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcRkZuGRwc-Lj3SZchav1pXSXHeHA1wYlGlrZeYP84H66AiMm0OyM9bk5x0i"
            };
            result.Add(driver3);

            return Json(result);
        }
    }

}