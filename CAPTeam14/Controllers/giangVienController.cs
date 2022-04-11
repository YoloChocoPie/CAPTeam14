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
        public ActionResult Index2()
        {
            var giangvien = model.nguoiDungs.OrderByDescending(x => x.ID).ToList();
            return View(giangvien);
        }

        [HttpGet]

        public JsonResult Details(int? id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID == id);
            string ten = giangvien.tenGV;
            string maGV = giangvien.maGV;
            string loaiGV = "";
            if (giangvien.loaiGV == true)
            {
                loaiGV = "Cơ hữu";
            }
            else
            {
                loaiGV = "Thỉnh giảng";
            }
            string role = "";
            if (giangvien.role == 1)
            {
                role = "Admin";
            }
            else if (giangvien.role == 2)
            {
                role = "Ban chủ nhiệm khoa";
            }
            else if (giangvien.role == 3)
            {
                role = "Bộ môn";
            }
            else if (giangvien.role == 4)
            {
                role = "Giảng viên";
            }
            else if (giangvien.role == null)
            {
                role = "Chưa kích hoạt";
            }
            string khoa = giangvien.khoa;
            int? sdt = giangvien.sdt;

            string gioiTinh = "";
            if (giangvien.gioiTinh == true)
            {
                gioiTinh = "Nam";
            }
            else
            {
                gioiTinh = "Nữ";
            }

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

                    if ((int)Session["id1"] == acc.ID)
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


        [HttpGet]
        [LoginVerification]
        public ActionResult DetailStatistic(int? id, int? idhk)
        {
            var cl = model.danhsachGVs.FirstOrDefault(x => x.ID == id);
            ViewBag.tengv = cl.tenGV;
            ViewBag.maid = cl.ID;
            //ViewBag.active = 7;
            //ViewBag.year = year;
            //int result = Int32.Parse(year);
            //ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).ToList().Count();
            //ViewBag.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;
            //ViewBag.sumsubhk = model.ACCOUNTs.Where(x => x.DATE_OF_REGISTRATION.Value.Year == result).ToList().Count();
            return View(cl);

        }

        [HttpGet]
        public ActionResult thongke(int? idgv, int? idhk, TKB tkb)
        {
            var cl = model.danhsachGVs.FirstOrDefault(x => x.ID == idgv);
            //đếm tổng số lớp trong 1 tuần
            ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).ToList().Count();

            // tính tổng giờ dạy trong 1 học kỳ
            //var listtuan = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).Select(x => x.tuanHoc.tuanHoc1).Distinct().ToList();
            //foreach(string tuan in listtuan)
            //{
            //    string[] cc = tuan.Split(',', ';', ' ');
            //    var ccc = cc.Count();
            //    ViewBag.sav = ccc;
            //}
            ViewBag.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;

            // đếm tổng số môn dạy trong 1 học kỳ
            ViewBag.sumsubhk = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).Select(x => x.ID_monHoc).Distinct().ToList().Count();
            //var abc = new { a = sumclassweek };
            var resu = new
            {
                tongloptuan = ViewBag.sumclassweek,
                tonggiohk = ViewBag.sumgghk,
                tongmonhk = ViewBag.sumsubhk
            };
            return Json(resu, JsonRequestBehavior.AllowGet);

        }


        // DetailStatistic cho giảng viên
        [HttpGet]
        [LoginVerification]
        public ActionResult DetailStatistic1(int? id, int? idhk)
        {

            ViewBag.tengv = Session["hoten"];
            ViewBag.maid = Session["id1"];
            //ViewBag.active = 7;
            //ViewBag.year = year;
            //int result = Int32.Parse(year);
            //ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).ToList().Count();
            //ViewBag.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;
            //ViewBag.sumsubhk = model.ACCOUNTs.Where(x => x.DATE_OF_REGISTRATION.Value.Year == result).ToList().Count();
            return View();

        }

        // DetailStatistic cho giảng viên
        [HttpGet]
        public ActionResult thongke1(int? idgv, int? idhk, TKB tkb)
        {

            var id = Convert.ToInt32(Session["id"]);
            //đếm tổng số lớp trong 1 tuần
            ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == id && x.ID_hocKy == idhk).ToList().Count();

            // tính tổng giờ dạy trong 1 học kỳ
            //var listtuan = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).Select(x => x.tuanHoc.tuanHoc1).Distinct().ToList();
            //foreach(string tuan in listtuan)
            //{
            //    string[] cc = tuan.Split(',', ';', ' ');
            //    var ccc = cc.Count();
            //    ViewBag.sav = ccc;
            //}
            ViewBag.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;

            // đếm tổng số môn dạy trong 1 học kỳ
            ViewBag.sumsubhk = model.TKBs.Where(x => x.ID_GV == id && x.ID_hocKy == idhk).Select(x => x.ID_monHoc).Distinct().ToList().Count();
            //var abc = new { a = sumclassweek };
            var resu = new
            {
                tongloptuan = ViewBag.sumclassweek,
                tonggiohk = ViewBag.sumgghk,
                tongmonhk = ViewBag.sumsubhk
            };
            return Json(resu, JsonRequestBehavior.AllowGet);

        }


        // DetailStatistic cho giảng viên
        [HttpGet]
        [LoginVerification]
        public ActionResult DetailStatisticAll(int? id)
        {
            //var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            //TempData["idhk"] = tkb1.ID;
            //ViewBag.tengv = Session["hoten"];
            //ViewBag.maid = Session["id1"];
            //ViewBag.active = 7;
            //ViewBag.year = year;
            //int result = Int32.Parse(year);
            //ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).ToList().Count();
            //ViewBag.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;
            //ViewBag.sumsubhk = model.ACCOUNTs.Where(x => x.DATE_OF_REGISTRATION.Value.Year == result).ToList().Count();
            return View();

        }

        // DetailStatistic cho giảng viên
        [HttpGet]
        public ActionResult thongkeAll(int? idhk, TKB tkb)
        {

            var gv = model.danhsachGVs.OrderByDescending(x => x.ID).ToList();
            //đếm tổng số lớp trong 1 tuần
            //for(var i = 0;  ;i++)
            //{
            //    ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == item.ID && x.ID_hocKy == idhk).ToList().Count();
            //}
            //ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).ToList().Count();

            // tính tổng giờ dạy trong 1 học kỳ
            //var listtuan = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).Select(x => x.tuanHoc.tuanHoc1).Distinct().ToList();
            //foreach(string tuan in listtuan)
            //{
            //    string[] cc = tuan.Split(',', ';', ' ');
            //    var ccc = cc.Count();
            //    ViewBag.sav = ccc;
            //}
            //.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;

            // đếm tổng số môn dạy trong 1 học kỳ
            //ViewBag.sumsubhk = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).Select(x => x.ID_monHoc).Distinct().ToList().Count();
            //var abc = new { a = sumclassweek };
            var resu = new
            {
                tongloptuan = ViewBag.sumclassweek,
                //tonggiohk = ViewBag.sumgghk,
                //tongmonhk = ViewBag.sumsubhk
            };
            return Json(resu, JsonRequestBehavior.AllowGet);

        }
    }
}



