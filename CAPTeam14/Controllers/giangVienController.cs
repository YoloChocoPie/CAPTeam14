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
    public class giangVienController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();
        // GET: giangVien

        public ActionResult Index()
        {
            var giangvien = model.nguoiDungs.OrderByDescending(x => x.ID).ToList();
            return View(giangvien);
        }
        /*[HttpGet]
        public ActionResult Edit(int id)
        {
            var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID == id);
            return View(giangvien);
        }*/
       /* [HttpPost]
        public ActionResult Edit(int id, nguoiDung acc)
        {
            try
            {
                var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID == id);

                giangvien.tenGV = acc.tenGV;
                giangvien.maGV = acc.maGV;
                giangvien.loaiGV = acc.loaiGV;
                giangvien.role = acc.role;
                giangvien.khoa = acc.khoa;
                giangvien.sdt = acc.sdt;
                giangvien.userID = acc.userID;
                giangvien.gioiTinh = acc.gioiTinh;
                model.SaveChanges();
                TempData["phanquyen"] = 1;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }*/

        [HttpGet]
        
        public JsonResult Details(int? id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID == id);
            string ten = giangvien.tenGV;
            string maGV = giangvien.maGV;
            bool? loaiGV = giangvien.loaiGV;
            int? role = giangvien.role;
            string khoa = giangvien.khoa;
            int? sdt = giangvien.sdt;
          
            bool? gioiTinh = giangvien.gioiTinh;

            var abcdefg = new { a = ten, b = maGV, c = loaiGV, d = role, e = khoa, f = sdt, g = gioiTinh };
            return Json(abcdefg, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        
        public ActionResult Edit(int? id, string email)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.nguoiDungs.FirstOrDefault(x => x.ID == id);
            
            return View(cl);
        }

        [HttpPost]
        
        public ActionResult Edit(int? id, nguoiDung acc)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
           /* ValidateClass(cl);*/

            try
            {
                if (ModelState.IsValid)
                {
                    var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID == id);

                    giangvien.tenGV = acc.tenGV;
                    giangvien.maGV = acc.maGV;
                    giangvien.loaiGV = acc.loaiGV;
                    giangvien.role = acc.role;
                    giangvien.khoa = acc.khoa;
                    giangvien.sdt = acc.sdt;
                    giangvien.userID = acc.userID;
                    giangvien.gioiTinh = acc.gioiTinh;

                    if ((int)Session["id"] == acc.ID)
                    {
                        TempData["phanquyen1"] = 1;
                        return RedirectToAction("Index");
                    }
                    model.SaveChanges();
                    TempData["phanquyen"] = 1;
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
           
            return View(acc);
        }




    }
}



