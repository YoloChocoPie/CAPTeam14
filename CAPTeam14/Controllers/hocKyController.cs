using CAPTeam14.Middleware;
using CAPTeam14.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTeam14.Controllers
{
    [LoginVerification]
    public class hocKyController : Controller
    {
        // GET: hocKy
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var hk = model.hocKies.OrderByDescending(x => x.ID).ToList();
            return View(hk);
        }

        //Tạo học kỳ mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(hocKy hk)
        {

            /*xacThuc(hp);*/
            try
            {
                if (ModelState.IsValid)
                {
                    var hocky1 = new hocKy();
                    hocky1.namBD = hk.namBD;
                    hocky1.namKT = hk.namKT;
                    hocky1.tenHK = hk.tenHK;

                    TempData["taoHK"] = 1;

                    model.hocKies.Add(hocky1);
                    model.SaveChanges();

                    return RedirectToAction("Index1", "Home");
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    ModelState.AddModelError("", messages);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Không thể thực hiện hành động này, vui lòng kiểm tra lại các trường thông tin");
            }
            return View(hk);

        }

        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var hocKy = model.hocKies.FirstOrDefault(x => x.ID == id);
            string tenhk = hocKy.tenHK;
            string nambd = hocKy.namBD;
            string namkt = hocKy.namKT;
            


            var abc = new { a = tenhk, b = nambd, c = namkt };
            return Json(abc, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.hocKies.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, hocKy hk)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";

            /*xacThuc1(hp);*/
            try
            {
                if (ModelState.IsValid)
                {
                    var hocky1 = model.hocKies.FirstOrDefault(x => x.ID == id);
                    hocky1.namBD = hk.namBD;
                    hocky1.namKT = hk.namKT;
                    hocky1.tenHK = hk.tenHK;


                    model.SaveChanges();
                    TempData["EditHK"] = 1;

                    return RedirectToAction("Index");
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    ModelState.AddModelError("", messages);
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Không thể lưu các thay đổi. Hãy thử lại và nếu sự cố vẫn tiếp diễn, hãy gặp quản trị viên hệ thống của bạn.");
            }

            return View(hk);
        }
    }
}