using CAPTeam14.Middleware;
using CAPTeam14.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTeam14.Controllers
{
    [LoginVerification]
    public class dsGVController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();
        // GET: dsGV
        public ActionResult Index()
        {
            var dsgv = model.danhsachGVs.OrderByDescending(x => x.ID).ToList();
            return View(dsgv);
        }

        [HttpGet]
        // Luồng đi mới của Import
        public ActionResult Catalog(int? id)
        {

            
            return View();
        }

        /// <summary>
        /// Code download templdate format thời khóa biểu
        /// </summary>
        /// <param name="Path"></param>
        /// <returns>file</returns>
        public FileResult DownloadExcel()
        {
            // nhớ sửa lại đường dẫn khi publish server
            string path = "/Doc/DSGiangVien.xlsx";
            return File(path, "application/vnd.ms-excel", "TemplateGiangVien.xlsx");
        }

        public ActionResult Catalog()
        {

            try
            {

                // khai báo thư viên IEDreader
                // khai báo thư viên EDSC
                IExcelDataReader IEDreader = ExcelReaderFactory.CreateOpenXmlReader(Request.Files[0].InputStream);
                DataSet dataS = IEDreader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (DataTable) => new ExcelDataTableConfiguration()
                    {
                        // Sử dụng cột đầu tiên làm tên của dữ liệu
                        UseHeaderRow = true,

                    }
                });
                // vòng lặp for each đi qua từng dữ liệu có trong file Excel
                foreach (DataTable item in dataS.Tables)
                {
                    // vòng lặp for each đi qua từng cột có trong excel để lấy dữ liệu
                    foreach (DataRow cot in item.Rows)
                    {
                        var dsgv = new danhsachGV(); // 1
                     





                        // Dữ liệu từng cột theo thứ tự

                        String maGV = cot[0].ToString();
                        String tenGV = cot[1].ToString();
                        String ngaysinh = cot[2].ToString(); // không dùng
                        String khoa = cot[3].ToString();
                        String loaiGV = cot[4].ToString();
                        

                        // table hocPhans
                        var checkmaGV = model.danhsachGVs.FirstOrDefault(x => x.maGV == maGV); // 1
                        var checktenGV = model.danhsachGVs.FirstOrDefault(x => x.tenGV == tenGV); // 2
                        var checkkhoa = model.danhsachGVs.FirstOrDefault(x => x.khoa == khoa); // 3
                        var checkloaiGV = model.danhsachGVs.FirstOrDefault(x => x.loaiGV == loaiGV); // 4
                       





                        // Học phần

                        // câu lệnh kiểm tra xem học phần đã tồn tại hay chưa
                        // nếu chưa tồn tại thì tạo mới Học Phần
                        if (checkmaGV == null)
                        {
                            dsgv = new danhsachGV
                            {
                                maGV = maGV,
                                tenGV = tenGV,
                                khoa = khoa,
                                loaiGV = loaiGV,
                                
                            };
                            model.danhsachGVs.Add(dsgv);
                            model.SaveChanges();

                        }
                        // còn nếu đã tồn tại một trong những dữ liệu trên rồi thì lưu giữ dữ liệu đã tồn tại và tiếp tục vòng lặp để kiểm tra 
                        // xem có dữ liệu nào khác thì
                        else
                        {
                            dsgv = checkmaGV;
                            dsgv = checktenGV;
                            dsgv = checkkhoa;
                            dsgv = checkloaiGV;
                            
                        }





                    }
                }
                // kết thúc vòng lặp và ngưng đọc dữ liệu sau 29 cột
                IEDreader.Close();
                TempData["thongbao3"] = 1;
                return RedirectToAction("Index", "dsGV");

            }

            catch (Exception)
            {
                ModelState.AddModelError("", "Không thể thực hiện hành động này, vui lòng kiểm tra File Excel có đúng định dạng");
            }
            return View();


        }

        [HttpGet]

        public JsonResult Details(int? id)
        {
            ViewBag.active = 11;
            model.Configuration.ProxyCreationEnabled = false;
            var ds = model.danhsachGVs.FirstOrDefault(x => x.ID == id);

            string ten = ds.tenGV;
            string maGV = ds.maGV;
            string loaiGV = ds.loaiGV;
            

            
            string khoa = ds.khoa;
          

            var abcd = new { a = ten, b = maGV, c = loaiGV, d = khoa};
            return Json(abcd, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.active = 11;
            ViewBag.tt = "Edit";
            var cl = model.danhsachGVs.FirstOrDefault(x => x.ID == id);
            
            return View(cl);
        }

        [HttpPost]
        public ActionResult Edit(int? id, danhsachGV ds)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ds1 = model.danhsachGVs.FirstOrDefault(x => x.ID == id);

                    ds1.khoa = ds.khoa;
                    ds1.loaiGV = ds.loaiGV;
                    ds1.maGV = ds.maGV;
                    ds1.tenGV = ds.tenGV;
                    
                    model.SaveChanges();
                    TempData["EditDS"] = 1;

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
           
            return View(ds);
        }
    }
}