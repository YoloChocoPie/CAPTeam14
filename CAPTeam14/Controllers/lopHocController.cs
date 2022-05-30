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
    public class lopHocController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var lophoc = model.lopHocs.OrderByDescending(x => x.ID).ToList();
            return View(lophoc);
        }
        //Tạo tuần học mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(lopHoc lh)
        {

            xacThuc(lh);
            try
            {
                if (ModelState.IsValid)
                {
                    var lophoc1 = new lopHoc();
                    lophoc1.maLop = lh.maLop;
                    

                    TempData["taolop"] = 1;

                    model.lopHocs.Add(lophoc1);
                    model.SaveChanges();

                    return RedirectToAction("Index", "lopHoc");
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
            return View(lh);

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


        private void xacThuc(lopHoc lh)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng
            var code = model.lopHocs.FirstOrDefault(a => a.maLop == lh.maLop);

            //Mã lớp
            if (lh.maLop == null)
            {
                ModelState.AddModelError("maLop", "Vui lòng nhập mã lớp");
            }
            else
            {
                if (code != null)
                {
                    ModelState.AddModelError("maLop", "Mã lớp đã tồn tại");
                }
                else
                {
                    // Test case nhập khoảng trắng
                    if (lh.maLop.Trim() == "")
                    {
                        ModelState.AddModelError("maLop", "Không được nhập khoảng trắng");
                    }
                }
                

            }

            
        }

        private void xacThuc2(lopHoc lh)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng
            var code = model.lopHocs.FirstOrDefault(a => a.maLop == lh.maLop);

            //Mã lớp
            if (lh.maLop == null)
            {
                ModelState.AddModelError("maLop", "Vui lòng nhập mã lớp");
            }
            else
            {
                
                
                    // Test case nhập khoảng trắng
                    if (lh.maLop.Trim() == "")
                    {
                        ModelState.AddModelError("maLop", "Không được nhập khoảng trắng");
                    }
                    
                


            }


        }


        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var malop = model.lopHocs.FirstOrDefault(x => x.ID == id);
            string ml = malop.maLop;

            var a = new { a = ml};
            return Json(a, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.lopHocs.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, lopHoc lh)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";

            xacThuc2(lh);
            try
            {
                if (ModelState.IsValid)
                {
                    var lophoc1 = model.lopHocs.FirstOrDefault(x => x.ID == id);

                    lophoc1.maLop = lh.maLop;


                    model.SaveChanges();
                    TempData["editlop"] = 1;

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

            return View(lh);
        }
    }
}