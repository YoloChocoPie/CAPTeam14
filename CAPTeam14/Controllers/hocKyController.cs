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

            xacThuc(hk);
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

                    return RedirectToAction("Index", "hocKy");
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

            xacThuc1(hk);
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

        //Hàm kiểm tra ký tự đặc biệt
        public static bool Kytudacbiet(string str)
        {
            //khai báo các ký tự đặc biệt
            string kytudacbiet = "^[0-9]+$";
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


        private void xacThuc(hocKy hk)
        {
            var code = model.hocKies.FirstOrDefault(d => d.tenHK == hk.tenHK);
            //Test case bỏ trống tên học kì
            if (hk.tenHK == null)
            {
                ModelState.AddModelError("tenHK", "Vui lòng nhập tên học kỳ");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (hk.tenHK.Trim() == "")
                {
                    ModelState.AddModelError("tenHK", "Không được nhập khoảng trắng");
                }
                if (code != null)
                {
                    ModelState.AddModelError("tenHK", "Tên học kì đã tồn tại");
                }
            }


            //
            //Test case bỏ trống năm bắt đầu
            if (hk.namBD == null)
            {
                ModelState.AddModelError("namBD", "Vui lòng nhập năm bắt đầu");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (hk.namBD.Trim() == "")
                {
                    ModelState.AddModelError("namBD", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(hk.namBD.Trim()) == true)
                    {
                        ModelState.AddModelError("namBD", "Không được nhập ký tự");
                    }
                }

            }

            ////
            //Test case bỏ trống năm kết thúc
            if (hk.namKT == null)
            {
                ModelState.AddModelError("namKT", "Vui lòng nhập năm kết thúc");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (hk.namKT.ToString().Trim() == "")
                {
                    ModelState.AddModelError("namKT", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(hk.namKT.Trim()) == true)
                    {
                        ModelState.AddModelError("namKT", "Không được nhập ký tự");
                    }
                }

            }
        }






        private void xacThuc1(hocKy hk)
        {
            var code = model.hocKies.FirstOrDefault(d => d.tenHK == hk.tenHK);
            //Test case bỏ trống tên học kì
            if (hk.tenHK == null)
            {
                ModelState.AddModelError("tenHK", "Vui lòng nhập tên học kỳ");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (hk.tenHK.Trim() == "")
                {
                    ModelState.AddModelError("tenHK", "Không được nhập khoảng trắng");
                }

            }


            //
            //Test case bỏ trống năm bắt đầu
            if (hk.namBD == null)
            {
                ModelState.AddModelError("namBD", "Vui lòng nhập năm bắt đầu");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (hk.namBD.Trim() == "")
                {
                    ModelState.AddModelError("namBD", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(hk.namBD.Trim()) == true)
                    {
                        ModelState.AddModelError("namBD", "Không được nhập ký tự");
                    }
                }

            }

            ////
            //Test case bỏ trống năm kết thúc
            if (hk.namKT == null)
            {
                ModelState.AddModelError("namKT", "Vui lòng nhập năm kết thúc");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (hk.namKT.ToString().Trim() == "")
                {
                    ModelState.AddModelError("namKT", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(hk.namKT.Trim()) == true)
                    {
                        ModelState.AddModelError("namKT", "Không được nhập ký tự");
                    }
                }

            }
        }
    }


    
}