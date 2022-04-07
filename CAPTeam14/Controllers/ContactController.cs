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
    public class ContactController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();
        // GET: Contact
        public ActionResult Index()
        {
            var contact = model.Contacts.OrderByDescending(x => x.ID).ToList();
            return View(contact);
        }
        public ActionResult Index1()
        {
            var contact = model.Contacts.OrderByDescending(x => x.ID).ToList();
            ViewBag.ten = (string)Session["hoten"];
            return View(contact);
        }


        //Tạo ticket
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact c)
        {

            xacThuc(c);
            try
            {
                if (ModelState.IsValid)
                {
                    var contact1 = new Contact();
                    contact1.maTicket = c.maTicket;
                    contact1.tenTicket = c.tenTicket;
                    contact1.ndTicket = c.ndTicket;
                    contact1.nguoigui = (string)Session["hoten"];
                    contact1.ID_vande = c.ID_vande;
                    contact1.trangthai = "Đã tiếp nhận";
                    contact1.admin = c.admin;
                    TempData["taoticket"] = 1;

                    model.Contacts.Add(contact1);
                    model.SaveChanges();

                    return RedirectToAction("Create", "Contact");
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
            return View(c);

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


        private void xacThuc(Contact lh)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng


            //tên ticket
            if (lh.tenTicket == null)
            {
                ModelState.AddModelError("tenTicket", "Vui lòng nhập nội dung tóm tắt");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (lh.tenTicket.Trim() == "")
                {
                    ModelState.AddModelError("tenTicket", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(lh.tenTicket.Trim()) == true)
                    {
                        ModelState.AddModelError("tenTicket", " không được có ký tự đặc biệt");
                    }
                }

            }
            //nội dung ticket
            if (lh.ndTicket == null)
            {
                ModelState.AddModelError("ndTicket", "Vui lòng nhập nội dung chi tiết");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (lh.tenTicket.Trim() == "")
                {
                    ModelState.AddModelError("ndTicket", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(lh.ndTicket.Trim()) == true)
                    {
                        ModelState.AddModelError("ndTicket", " không được có ký tự đặc biệt");
                    }
                }

            }


        }




        [HttpGet]
        public JsonResult Details(int id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var contact = model.Contacts.FirstOrDefault(x => x.ID == id);
            string contac1 = contact.maTicket;
            string contact2 = contact.tenTicket;
            string contact3 = contact.ndTicket;
            string contact4 = contact.nguoigui;
            string contact5 = "";
            if (contact.ID_vande == 1)
            {
                contact5 = "Tài khoản";
            }
            else if (contact.ID_vande == 2)
            {
                contact5 = "Quyền";
            }
            else if (contact.ID_vande == 3)
            {
                contact5 = "Thời khóa biểu";
            }
            else if (contact.ID_vande == 4)
            {
                contact5 = "Lỗi hiển thị";
            }
            else if (contact.ID_vande == 5)
            {
                contact5 = "Khác";
            }
            
            string contact6 = contact.trangthai;




            var abcdef = new { a = contac1, b = contact2, c = contact3, d = contact4, e = contact5, f = contact6 };
            return Json(abcdef, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {

            var cl = model.Contacts.FirstOrDefault(x => x.ID == id);
            ViewBag.vande = cl.vande.tenVande;
            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, Contact c)
        {

            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.Contacts.FirstOrDefault(x => x.ID == id);
            ViewBag.vande = cl.vande.tenVande;
            ViewBag.vande1 = cl.ID_vande;

            xacThuc1(c);
            try
            {
                if (ModelState.IsValid)
                {
                    var contact1 = model.Contacts.FirstOrDefault(x => x.ID == id);
                    contact1.maTicket = c.maTicket;
                    contact1.tenTicket = c.tenTicket;
                    contact1.ndTicket = c.ndTicket;
                    contact1.nguoigui = c.nguoigui;
                    contact1.ID_vande = ViewBag.vande1;
                    contact1.trangthai = c.trangthai;
                    contact1.admin = (string)Session["hoten"];


                    model.SaveChanges();
                    TempData["EditContact"] = 1;

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
           
            return View(c);
        }

        private void xacThuc1(Contact lh)
        {
            // kiểm tra xem mã liên học phần và mã gốc liên học phần đã tồn tại hay chưa
            // Các trường hợp cover :
            // - Không được bỏ trống
            // - Không bị trùng
            // - Không nhập khoảng trắng


            //tên ticket
            if (lh.tenTicket == null)
            {
                ModelState.AddModelError("tenTicket", "Vui lòng nhập nội dung tóm tắt");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (lh.tenTicket.Trim() == "")
                {
                    ModelState.AddModelError("tenTicket", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(lh.tenTicket.Trim()) == true)
                    {
                        ModelState.AddModelError("tenTicket", " không được có ký tự đặc biệt");
                    }
                }

            }
            //nội dung ticket
            if (lh.ndTicket == null)
            {
                ModelState.AddModelError("ndTicket", "Vui lòng nhập nội dung chi tiết");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (lh.tenTicket.Trim() == "")
                {
                    ModelState.AddModelError("ndTicket", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test case kiểm tra kí tự đặc biệt
                    if (Kytudacbiet(lh.ndTicket.Trim()) == true)
                    {
                        ModelState.AddModelError("ndTicket", " không được có ký tự đặc biệt");
                    }
                }

            }
        }
    }
}