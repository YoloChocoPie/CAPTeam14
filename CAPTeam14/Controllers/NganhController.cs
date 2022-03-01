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
    public class NganhController : Controller
    {
        // GET: Nganh
        CP24Team14Entities model = new CP24Team14Entities();
        public ActionResult Index()
        {
            var nganh = model.Nganhs.OrderByDescending(x => x.ID).ToList();
            return View(nganh);
        }


        //Tạo ngành mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nganh nganh)
        {

            xacThuc(nganh);
            try
            {
                if (ModelState.IsValid)
                {
                    var nganhhoc = new Nganh();
                    nganhhoc.maNganh = nganh.maNganh;
                    nganhhoc.tenNganh = nganh.tenNganh;
                  
                    TempData["taoNganh"] = 1;

                    model.Nganhs.Add(nganhhoc);
                    model.SaveChanges();

                    return RedirectToAction("Index", "Nganh");
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
            return View(nganh);

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


        private void xacThuc(Nganh nganh)
        {
            var code = model.Nganhs.FirstOrDefault(d => d.maNganh == nganh.maNganh);
            //Test case bỏ trống mã ngành
            if (nganh.maNganh == null)
            {
                ModelState.AddModelError("maNganh", "Vui lòng nhập mã ngành học");
            }
            else
            {
                if (code != null)
                {
                    ModelState.AddModelError("maNganh", "Mã ngành đã tồn tại");
                }
                // Test case nhập khoảng trắng
                if (nganh.maNganh.ToString().Trim() == "")
                {
                    ModelState.AddModelError("maNganh", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(nganh.maNganh.ToString().Trim()) == true)
                    {
                        ModelState.AddModelError("maNganh", "Mã ngành không được có ký tự đặc biệt");
                    }
                }
            }

            //
            //Test case bỏ trống tên ngành
            if (nganh.tenNganh == null)
            {
                ModelState.AddModelError("tenNganh", "Vui lòng nhập tên ngành ");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (nganh.tenNganh.Trim() == "")
                {
                    ModelState.AddModelError("tenNganh", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(nganh.tenNganh.ToString().Trim()) == true)
                    {
                        ModelState.AddModelError("tenNganh", "Tên ngành không được có ký tự đặc biệt");
                    }
                }

            }
        }


        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var nganh = model.Nganhs.FirstOrDefault(x => x.ID == id);
            string ten = nganh.tenNganh;
            string ma = nganh.maNganh;
            
            var ab = new { a = ten, b = ma};
            return Json(ab, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.Nganhs.FirstOrDefault(x => x.ID == id);

            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, Nganh nganh)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            
            xacThuc(nganh);
            try
            {
                if (ModelState.IsValid)
                {
                    var nganhhoc = model.Nganhs.FirstOrDefault(x => x.ID == id);
                    nganhhoc.tenNganh = nganh.tenNganh;
                    nganhhoc.maNganh = nganh.maNganh;
                    

                    model.SaveChanges();
                    TempData["nganhHoc"] = 1;

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

            return View(nganh);
        }
    
    }
}