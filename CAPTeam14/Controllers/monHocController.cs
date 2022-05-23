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
    public class monHocController : Controller
    {
        // GET: monHoc
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var monhoc = model.monHocs.OrderByDescending(x => x.ID).ToList();
            return View(monhoc);
        }


        //Tạo môn
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(monHoc mon)
        {
          
            xacThuc(mon);
            try
            {
                if (ModelState.IsValid)
                {
                    var monHoc = new monHoc();
                    monHoc.tenMon = mon.tenMon;
                    monHoc.maMon = mon.maMon;
                    monHoc.tinChi = mon.tinChi;
                    monHoc.tengoiKhac = mon.tengoiKhac;
                    TempData["taoMon"] = 1;
                
                    model.monHocs.Add(monHoc);
                    model.SaveChanges();

                    return RedirectToAction("Index", "monHoc");
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
            return View(mon);

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


        private void xacThuc(monHoc mon)
        {
            var code = model.monHocs.FirstOrDefault(d => d.maMon == mon.maMon );
            //Test case bỏ trống họ tên
            if (mon.tenMon == null)
            {
                ModelState.AddModelError("tenMon", "Vui lòng nhập tên môn học");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (mon.tenMon.Trim() == "")
                {
                    ModelState.AddModelError("tenMon", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(mon.tenMon.Trim()) == true)
                    {
                        ModelState.AddModelError("tenMon", "Họ tên không được có ký tự đặc biệt");
                    }
                }
            }


            //
            //Test case bỏ trống mã môn
            if (mon.maMon == null)
            {
                ModelState.AddModelError("maMon", "Vui lòng nhập mã môn");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (mon.maMon.Trim() == "")
                {
                    ModelState.AddModelError("maMon", "Không được nhập khoảng trắng");
                }
                else
                {
                    if (code != null)
                    {
                        ModelState.AddModelError("maMon", "Mã môn đã tồn tại");
                    }
                }
            }

            ////
            //Test case bỏ trống tín chỉ
            if (mon.tinChi == null)
            {
                ModelState.AddModelError("tinChi", "Vui lòng nhập số tín chỉ");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (mon.tinChi.ToString().Trim() == "")
                {
                    ModelState.AddModelError("tinChi", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(mon.tinChi.ToString().Trim()) == true)
                    {
                        ModelState.AddModelError("tinChỉ", "Số tín chỉ không được có ký tự đặc biệt");
                    }
                }
            }
        }


        private void xacThuc2(monHoc mon)
        {
            var code = model.monHocs.FirstOrDefault(d => d.maMon == mon.maMon);
            //Test case bỏ trống họ tên
            if (mon.tenMon == null)
            {
                ModelState.AddModelError("tenMon", "Vui lòng nhập tên môn học");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (mon.tenMon.Trim() == "")
                {
                    ModelState.AddModelError("tenMon", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(mon.tenMon.Trim()) == true)
                    {
                        ModelState.AddModelError("tenMon", "Họ tên không được có ký tự đặc biệt");
                    }
                }
            }


            //
            //Test case bỏ trống mã môn
            if (mon.maMon == null)
            {
                ModelState.AddModelError("maMon", "Vui lòng nhập mã môn");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (mon.maMon.Trim() == "")
                {
                    ModelState.AddModelError("maMon", "Không được nhập khoảng trắng");
                }
                
            }

            ////
            //Test case bỏ trống tín chỉ
            if (mon.tinChi == null)
            {
                ModelState.AddModelError("tinChi", "Vui lòng nhập số tín chỉ");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (mon.tinChi.ToString().Trim() == "")
                {
                    ModelState.AddModelError("tinChi", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(mon.tinChi.ToString().Trim()) == true)
                    {
                        ModelState.AddModelError("tinChi", "Số tín chỉ không được có ký tự đặc biệt");
                    }
                }
            }
        }




        [HttpGet]

        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var monHoc = model.monHocs.FirstOrDefault(x => x.ID == id);
            string ten = monHoc.tenMon;
            string maMon = monHoc.maMon;
            string tinChi = monHoc.tinChi;
            string tenKhac = "";
            if (monHoc.tengoiKhac == null)
            {
                tenKhac = "Không có";
            }
            else
            {
                tenKhac = monHoc.tengoiKhac;
            }    
            

            

            var abcd= new { a = ten, b = maMon, c = tinChi, d = tenKhac };
            return Json(abcd, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.monHocs.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]

        public ActionResult Edit(int? id, monHoc mon)
        {
           
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            /* ValidateClass(cl);*/
            xacThuc2(mon);
            try
            {
                if (ModelState.IsValid)
                {
                    var monHoc = model.monHocs.FirstOrDefault(x => x.ID == id);
                    monHoc.tenMon = mon.tenMon;
                    monHoc.maMon = mon.maMon;
                    monHoc.tinChi = mon.tinChi;
                    monHoc.tengoiKhac = mon.tengoiKhac;
                   
                    model.SaveChanges();
                    TempData["monHoc"] = 1;
                    
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

            return View(mon);
        }
    }
}