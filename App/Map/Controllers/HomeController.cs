using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Core;
using Interfaces;
using Infrastructure.Data;

namespace Map.Controllers
{
    public class HomeController : Controller
    {
        //в качестве источника данных используются таблицы Excel
        readonly ISubdivisionRepository repo = new ExcelSubdivisionRepository(@"E:\Саша\Начало\Наполнение\Книга_с_группировкой_(2).xlsx");
        IEnumerable<Subdivision> subd;

        public ActionResult Index()
        {
            subd = repo.GetSubdivisions();
            return View(subd);
        }

        //отрисовка частичного представления(предполагалось перерисовывать только карту,если изменияется дата, а не перезагружать всю страницу)
        [HttpPost]
        public ActionResult SVG(IEnumerable<Subdivision> subdivisions)
        {
            return PartialView(subdivisions);
        }

        
       public ActionResult Map()
        {
            subd = repo.GetSubdivisions();
            return View(subd);
        }
        public JsonResult GetJson()
        {
            subd = repo.GetSubdivisions();
            return Json(subd,JsonRequestBehavior.AllowGet);
        }
    }
}