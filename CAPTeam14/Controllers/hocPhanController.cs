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
    public class hocPhanController : Controller
    {
        // GET: hocPhan
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var hp = model.hocPhans.OrderByDescending(x => x.ID).ToList();
            return View(hp);
        }

        //Tạo học phần mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(hocPhan hp)
        {

            xacThuc(hp);
            try
            {
                if (ModelState.IsValid)
                {
                    var hocPhan1 = new hocPhan();
                    hocPhan1.maGocLHP = hp.maGocLHP;
                    hocPhan1.maLHP = hp.maLHP;
                    hocPhan1.loaiHP = hp.loaiHP;                  
                    hocPhan1.tinhtrangLHP = hp.tinhtrangLHP;
                    hocPhan1.TSMH = hp.TSMH;

                    TempData["taoHP"] = 1;

                    model.hocPhans.Add(hocPhan1);
                    model.SaveChanges();

                    return RedirectToAction("Index", "hocPhan");
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
            return View(hp);

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


        private void xacThuc(hocPhan hp)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng

            var code = model.hocPhans.FirstOrDefault(d => d.maLHP == hp.maLHP && d.maGocLHP == hp.maGocLHP );
            //Mã Gốc Liên Học Phần
            if (hp.maGocLHP == null)
            {
                ModelState.AddModelError("maGocLHP", "Vui lòng nhập mã gốc liên học phần");
            }
            else
            {
                if (code != null)
                {
                    ModelState.AddModelError("maGocLHP", "Mã gốc liên học phần đã tồn tại");
                }
                // Test case nhập khoảng trắng

                if (hp.maGocLHP.Trim() == "")
                {
                    ModelState.AddModelError("maGocLHP", "Không được nhập khoảng trắng");
                }
               
            }

            //Mã Liên Học Phần
            if (hp.maLHP == null)
            {
                ModelState.AddModelError("maLHP", "Vui lòng nhập mã liên học phần");
            }
            else
            {
                if (code != null)
                {
                    ModelState.AddModelError("maLHP", "Mã liên học phần đã tồn tại");
                }
                // Test case nhập khoảng trắng

                if (hp.maLHP.Trim() == "")
                {
                    ModelState.AddModelError("maLHP", "Không được nhập khoảng trắng");
                }

            }

            //Loại Học Phần
            if (hp.loaiHP == null)
            {
                ModelState.AddModelError("loaiHP", "Vui lòng chọn loại học phần");
            }
            else
            {             
                // Test case nhập khoảng trắng

                if (hp.loaiHP.Trim() == "")
                {
                    ModelState.AddModelError("loaiHP", "Không được nhập khoảng trắng");
                }

            }

            //Tình Trạng Học Phần
            if (hp.tinhtrangLHP == null)
            {
                ModelState.AddModelError("tinhtrangLHP", "Vui lòng chọn tình trạng Liên Học Phần");
            }
            else
            {
                // Test case nhập khoảng trắng

                if (hp.tinhtrangLHP.Trim() == "")
                {
                    ModelState.AddModelError("tinhtrangLHP", "Không được nhập khoảng trắng");
                }

            }

            //Tổng số môn học
            if (hp.TSMH == null)
            {
                ModelState.AddModelError("TSMH", "Vui lòng nhập tổng số môn học");
            }
            else
            {
              
                // Test case nhập khoảng trắng

                if (hp.TSMH.ToString().Trim() == "")
                {
                    ModelState.AddModelError("maLHP", "Không được nhập khoảng trắng");
                }
            }
        }


        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var hocPhan = model.hocPhans.FirstOrDefault(x => x.ID == id);
            string magoc = hocPhan.maGocLHP;
            string ma = hocPhan.maLHP;
            string loai = hocPhan.loaiHP;
            string tinhtrang = hocPhan.tinhtrangLHP;
            string tong = hocPhan.TSMH;


            var abcde = new { a = magoc, b = ma, c = loai, d = tinhtrang, e = tong };
            return Json(abcde, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.hocPhans.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, hocPhan hp)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";

            xacThuc1(hp);
            try
            {
                if (ModelState.IsValid)
                {
                    var hocPhan1 = model.hocPhans.FirstOrDefault(x => x.ID == id);
                    hocPhan1.loaiHP = hp.loaiHP;
                    hocPhan1.maGocLHP = hp.maGocLHP;
                    hocPhan1.maLHP = hp.maLHP;
                    hocPhan1.tinhtrangLHP = hp.tinhtrangLHP;
                    hocPhan1.TSMH = hp.TSMH;


                    model.SaveChanges();
                    TempData["EditHP"] = 1;

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

            return View(hp);
        }


        private void xacThuc1(hocPhan hp)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng

            
            //Mã Gốc Liên Học Phần
            if (hp.maGocLHP == null)
            {
                ModelState.AddModelError("maGocLHP", "Vui lòng nhập mã gốc liên học phần");
            }
            else
            {
               
                // Test case nhập khoảng trắng

                if (hp.maGocLHP.Trim() == "")
                {
                    ModelState.AddModelError("maGocLHP", "Không được nhập khoảng trắng");
                }

            }

            //Mã Liên Học Phần
            if (hp.maLHP == null)
            {
                ModelState.AddModelError("maLHP", "Vui lòng nhập mã liên học phần");
            }
            else
            {
                
                // Test case nhập khoảng trắng

                if (hp.maLHP.Trim() == "")
                {
                    ModelState.AddModelError("maLHP", "Không được nhập khoảng trắng");
                }

            }

            //Loại Học Phần
            if (hp.loaiHP == null)
            {
                ModelState.AddModelError("loaiHP", "Vui lòng chọn loại học phần");
            }
            else
            {
                // Test case nhập khoảng trắng

                if (hp.loaiHP.Trim() == "")
                {
                    ModelState.AddModelError("loaiHP", "Không được nhập khoảng trắng");
                }

            }

            //Tình Trạng Học Phần
            if (hp.tinhtrangLHP == null)
            {
                ModelState.AddModelError("tinhtrangLHP", "Vui lòng chọn tình trạng Liên Học Phần");
            }
            else
            {
                // Test case nhập khoảng trắng

                if (hp.tinhtrangLHP.Trim() == "")
                {
                    ModelState.AddModelError("tinhtrangLHP", "Không được nhập khoảng trắng");
                }

            }

            //Tổng số môn học
            if (hp.TSMH == null)
            {
                ModelState.AddModelError("TSMH", "Vui lòng nhập tổng số môn học");
            }
            else
            {

                // Test case nhập khoảng trắng

                if (hp.TSMH.ToString().Trim() == "")
                {
                    ModelState.AddModelError("maLHP", "Không được nhập khoảng trắng");
                }
            }
        }
    }
}