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
    public class tuanHocController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var tuanHoc = model.tuanHocs.OrderByDescending(x => x.ID).ToList();
            return View(tuanHoc);
        }
        //Tạo tuần học mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tuanHoc th)
        {

            xacThuc(th);
            try
            {
                if (ModelState.IsValid)
                {
                    var tuanhoc1 = new tuanHoc();
                    tuanhoc1.thuS = th.thuS;
                    tuanhoc1.tuanBD = th.tuanBD;
                    tuanhoc1.tuanHoc1 = th.tuanHoc1;
                    tuanhoc1.tuanKT = th.tuanKT;
                    tuanhoc1.thu = th.thu;

                    TempData["taotuan"] = 1;

                    model.tuanHocs.Add(tuanhoc1);
                    model.SaveChanges();

                    return RedirectToAction("Index", "tuanHoc");
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
            return View(th);

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


        private void xacThuc(tuanHoc th)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng

            
            //Thứ bắt đầu
            if (th.thuS == null)
            {
                ModelState.AddModelError("thuS", "Vui lòng nhập thứ bắt đầu");
            }
            else
            {
                
                // Test case nhập khoảng trắng

                if (th.thuS.ToString().Trim() == "")
                {
                    ModelState.AddModelError("thuS", "Không được nhập khoảng trắng");
                }

            }

            //Tuần bắt đầu
            if (th.tuanBD == null)
            {
                ModelState.AddModelError("tuanBD", "Vui lòng nhập tuần bắt đầu");
            }
            else
            {

                // Test case nhập khoảng trắng

                if (th.tuanBD.ToString().Trim() == "")
                {
                    ModelState.AddModelError("tuanBD", "Không được nhập khoảng trắng");
                }

            }

            //Tuần học
            if (th.tuanHoc1 == null)
            {
                ModelState.AddModelError("tuanHoc1", "Vui lòng nhập tuần học");
            }
            else
            {
                // Test case nhập khoảng trắng

                if (th.tuanHoc1.Trim() == "")
                {
                    ModelState.AddModelError("tuanHoc1", "Không được nhập khoảng trắng");
                }

            }

            //Tuần kiểm tra
            if (th.tuanKT == null)
            {
                ModelState.AddModelError("tuanKT", "Vui lòng nhập tuần kiểm tra");
            }
            else
            {

                // Test case nhập khoảng trắng

                if (th.tuanKT.ToString().Trim() == "")
                {
                    ModelState.AddModelError("tuanKT", "Không được nhập khoảng trắng");
                }

            }

            //Thứ
            if (th.thu == null)
            {
                ModelState.AddModelError("thu", "Vui lòng nhập thứ");
            }
            else
            {

                // Test case nhập khoảng trắng

                if (th.thu.Trim() == "")
                {
                    ModelState.AddModelError("thu", "Không được nhập khoảng trắng");
                }

            }
        }


        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var tuanHoc = model.tuanHocs.FirstOrDefault(x => x.ID == id);
            string thuS = tuanHoc.thuS;
            string tuanBD = tuanHoc.tuanBD;
            string th = tuanHoc.tuanHoc1;
            string tuanKT = tuanHoc.tuanKT;
            string thu = tuanHoc.thu;
        


            var abcde = new { a = thuS, b = tuanBD, c = th, d = tuanKT, e = thu };
            return Json(abcde, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.tuanHocs.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, tuanHoc th)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";

            xacThuc(th);
            try
            {
                if (ModelState.IsValid)
                {
                    var tuanhoc1 = model.tuanHocs.FirstOrDefault(x => x.ID == id);
                    
                    tuanhoc1.thuS = th.thuS;
                    tuanhoc1.tuanBD = th.tuanBD;
                    tuanhoc1.tuanHoc1 = th.tuanHoc1;
                    tuanhoc1.tuanKT = th.tuanKT;
                    tuanhoc1.thu = th.thu;


                    model.SaveChanges();
                    TempData["edittuan"] = 1;

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

            return View(th);
        }


    }
}