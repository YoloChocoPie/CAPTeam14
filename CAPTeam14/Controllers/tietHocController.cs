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
    public class tietHocController : Controller
    {

        // GET: tietHoc
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var tiet = model.tietHocs.OrderByDescending(x => x.ID).ToList();
            return View(tiet);
        }


        //Thêm tiết học mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tietHoc tiet)
        {

            xacThuc(tiet);
            try
            {
                if (ModelState.IsValid)
                {
                    var th = new tietHoc();
                    th.tongTiet = tiet.tongTiet;
                    th.soTiet = tiet.soTiet;
                    th.tietHoc1 = tiet.tietHoc1;
                    th.tietS = tiet.tietS;
                    th.tietBD = tiet.tietBD;
                    

                    TempData["taoTiet"] = 1;

                    model.tietHocs.Add(th);
                    model.SaveChanges();

                    return RedirectToAction("Index", "tietHoc");
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
            return View(tiet);

        }

        //Hàm kiểm tra ký tự đặc biệt
        public static bool Kytudacbiet(string str)
        {
            //khai báo các ký tự đặc biệt
            string kytudacbiet = @"%!@#$%^&*()?/><:'\|}]{[_~`+=-" + "\"";
            //chuyển các ký tự đặc biệt sang dạng chuỗi
            char[] chuoikytudacbiet = kytudacbiet.ToCharArray();
            //kiểm tra trường thông tin người dùng nhập vào có chứa ký tự đặc biệt hay không
            int index = str.IndexOfAny(chuoikytudacbiet);
            // nếu index == -1 thì trả về false => không có ký tự đặc biệt
            if (index == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Chỉ cover trường hợp không thể bỏ trống
        private void xacThuc(tietHoc tiet)
        {

            //Test case bỏ trống mã phòng
            if (tiet.tongTiet == null)
            {
                ModelState.AddModelError("tongTiet", "Vui lòng số tổng tiết");
            }

            //
            //Test case bỏ trống sucChua
            if (tiet.soTiet == null)
            {
                ModelState.AddModelError("soTiet", "Vui lòng nhập số tiết ");
            }

            //Test case bỏ trống siSo
            if (tiet.tietHoc1 == null)
            {
                ModelState.AddModelError("tietHoc1", "Vui lòng nhập tiết học ");
            }

            //Test case bỏ trống siSo
            if (tiet.tietS == null)
            {
                ModelState.AddModelError("tietS", "Vui lòng nhập tiết đầu tiên ");
            }

            //Test case bỏ trống siSo
            if (tiet.tietBD == null)
            {
                ModelState.AddModelError("tietBD", "Vui lòng nhập số tiết bắt đầu ");
            }
        }


        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var tiet = model.tietHocs.FirstOrDefault(x => x.ID == id);


            string tong = tiet.tongTiet;
            string so = tiet.soTiet;
            string tiethoc2 = tiet.tietHoc1;
            string tietS = tiet.tietS;
            string tietBD = tiet.tietBD;

            var abcde = new { a = tong, b = so, c = tiethoc2, d = tietS, e = tietBD };
            return Json(abcde, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.tietHocs.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, tietHoc tiet)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";

            xacThuc(tiet);
            try
            {
                if (ModelState.IsValid)
                {
                    var th = model.tietHocs.FirstOrDefault(x => x.ID == id);
                    th.tongTiet = tiet.tongTiet;
                    th.soTiet = tiet.soTiet;
                    th.tietHoc1 = tiet.tietHoc1;
                    th.tietS = tiet.tietS;
                    th.tietBD = tiet.tietBD;


                    model.SaveChanges();
                    TempData["tietHoc"] = 1;

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

            return View(tiet);
        }
    }
}