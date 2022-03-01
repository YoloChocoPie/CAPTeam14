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
    public class phongHocController : Controller
    {
        // GET: phongHoc
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var phong = model.phongHocs.OrderByDescending(x => x.ID).ToList();
            return View(phong);
        }


        //Thêm phòng học mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(phongHoc phong)
        {

            xacThuc(phong);
            try
            {
                if (ModelState.IsValid)
                {
                    var ph = new phongHoc();
                    ph.maPhong = phong.maPhong;
                    ph.loaiPhong = phong.loaiPhong;
                    ph.sucChua = phong.sucChua;
                    ph.siSo = phong.siSo;
                    ph.trong = phong.trong;
                    ph.soSVDK = phong.soSVDK;

                    TempData["taoPhong"] = 1;

                    model.phongHocs.Add(ph);
                    model.SaveChanges();

                    return RedirectToAction("Index", "phongHoc");
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
            return View(phong);

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
        private void xacThuc(phongHoc phong)
        {
            
            //Test case bỏ trống mã phòng
            if (phong.maPhong == null)
            {
                ModelState.AddModelError("maPhong", "Vui lòng nhập mã phòng học");
            }

            //
            //Test case bỏ trống sucChua
            if (phong.sucChua == null)
            {
                ModelState.AddModelError("sucChua", "Vui lòng nhập số lượng sức chứa ");
            }

            //Test case bỏ trống siSo
            if (phong.siSo == null)
            {
                ModelState.AddModelError("siSo", "Vui lòng nhập số lượng sỉ số ");
            }

            //Test case bỏ trống siSo
            if (phong.trong == null)
            {
                ModelState.AddModelError("trong", "Vui lòng nhập số lượng trống ");
            }

            //Test case bỏ trống siSo
            if (phong.soSVDK == null)
            {
                ModelState.AddModelError("soSVDK", "Vui lòng nhập số lượng sinh viên đăng kí ");
            }
        }


        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var phong = model.phongHocs.FirstOrDefault(x => x.ID == id);
            string ma = phong.maPhong;
            string loai = phong.loaiPhong;

            string suc = phong.sucChua;
            string siso = phong.siSo;
            string trong = phong.trong;
            string svdk = phong.soSVDK;

            var abcdef = new { a = ma, b = loai, c = suc, d = siso, e = trong, f = svdk };
            return Json(abcdef, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.phongHocs.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, phongHoc phong)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";

            xacThuc(phong);
            try
            {
                if (ModelState.IsValid)
                {
                    var ph = model.phongHocs.FirstOrDefault(x => x.ID == id);
                    ph.maPhong = phong.maPhong;
                    ph.loaiPhong = phong.loaiPhong;
                    ph.sucChua = phong.sucChua;
                    ph.siSo = phong.siSo;
                    ph.trong = phong.trong;
                    ph.soSVDK = phong.soSVDK;


                    model.SaveChanges();
                    TempData["phongHoc"] = 1;

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

            return View(phong);
        }
    }
}