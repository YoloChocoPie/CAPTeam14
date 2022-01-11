using CAPTeam14.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAPTeam14.Models;
using System.Data.OleDb;
using System.Data;
using LinqToExcel;
using System.Data.Entity.Validation;

namespace CAPTeam14.Controllers
{
    [LoginVerification]
    public class HomeController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();

        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Courses()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Catalog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// This function is used to download excel format.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns>file</returns>
        public FileResult DownloadExcel()
        {
            string path = "/Doc/Users.xls";
            return File(path, "application/vnd.ms-excel", "test.xls");
        }

        [HttpPost]
        public JsonResult UploadExcel(HttpPostedFileBase FileUpload)
        {

            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";

                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }

                    // Không sử dụng được định đạng .xlsx
                    //
                    //

                    else if (filename.EndsWith(".xlsx"))
                    {
                        data.Add("Vui lòng chọn File Excel theo mẫu ở trên");

                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                    //
                    //
                    //

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] ", connectionString);
                    var ds = new DataSet();
                    adapter.Fill(ds, "ExcelTable");
                    DataTable dtable = ds.Tables["ExcelTable"];
                    string sheetName = "Sheet1";
                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    // khởi tạo thời khóa biểu để lưu khóa ngoại
                   
                    // HỌC PHẦN
                    //
                    //
                    var hocphan = from a in excelFile.Worksheet<hocPhan>(sheetName) select a;

                    foreach (var a in hocphan)
                    {
                        try
                        {
                            if (a.maGocLHP != "" && a.maLHP != "" && a.loaiHP != "" && a.tinhtrangLHP != "" && a.TSMH.ToString() != ""
                                )
                            {
                                
                                //
                                //

                                //Nếu mã gốc liên học phần đã tồn tại, thì bỏ qua 
                                
                                if (model.hocPhans.ToList().Any(o => o.maGocLHP == a.maGocLHP && o.maLHP == a.maLHP ))
                                {

                                }
                                else
                                {
                                    hocPhan TU = new hocPhan();
                                   /* monHoc TU1 = new monHoc();
                                    lopHoc TU2 = new lopHoc();
                                    tietHoc TU3 = new tietHoc();
                                    phongHoc TU4 = new phongHoc();
                                    Nganh TU5 = new Nganh();
                                    tuanHoc TU6 = new tuanHoc();

                                    TU.monHoc = TU1;
                                    TU.lopHoc = TU2;
                                    TU.tietHoc1 = TU3;
                                    TU.phongHoc = TU4;
                                    TU.Nganh = TU5;
                                    TU.tuanHoc1 = TU6;*/




                                    TU.maGocLHP = a.maGocLHP;
                                    TU.maLHP = a.maLHP;
                                    TU.loaiHP = a.loaiHP;
                                    TU.tinhtrangLHP = a.tinhtrangLHP;
                                    TU.TSMH = a.TSMH;

                                  /*  model.monHocs.Add(TU1);
                                    model.lopHocs.Add(TU2);
                                    model.tietHocs.Add(TU3);
                                    model.phongHocs.Add(TU4);
                                    model.Nganhs.Add(TU5);
                                    model.tuanHocs.Add(TU6);*/

                                    model.hocPhans.Add(TU);
                                    model.SaveChanges();
                                }
                                

                                //
                                //
                                //

                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.maGocLHP == "" || a.maGocLHP == null) data.Add("<li> Mã gốc LHP is required</li>");
                                if (a.maLHP == "" || a.maLHP == null) data.Add("<li> Mã LHP is required</li>");
                                if (a.loaiHP == "" || a.loaiHP == null) data.Add("<li> Loại Học Phần is required</li>");
                                if (a.tinhtrangLHP == "" || a.tinhtrangLHP == null) data.Add("<li> Tình trạng liên học phần is required</li>");
                                if (a.TSMH.ToString() == "" || a.TSMH.ToString() == null) data.Add("<li> Tổng số môn học is required</li>");
                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }
                    // MÔN HỌC
                    //
                    //

                    var monhoc = from a in excelFile.Worksheet<monHoc>(sheetName) select a;
                    foreach (var a in monhoc)
                    {
                        try
                        {
                            if (a.maMon != "" && a.tenMon != "" && a.tinChi.ToString() != ""
                                )
                            {
                                //
                                //
                                //
                                //Tạo hàm kiểm tra có tồn tại hay chưa 
                                

                                

                                // Nếu mã môn VÀ tên môn tồn tại tồn tại thì skip
                                // Hoặc nếu mã môn hoặc tên môn khác nhau thì add vô
                                // bây giờ là 00:24 ngày 11/01/2022. LTC vừa mất 4 tiếng cuộc đời
                                // Chỉ đê viết ra 14 dòng lệnh thỏa test case
                                if (model.monHocs.ToList().Any(o => o.maMon == a.maMon && o.tenMon == a.tenMon ))
                                {
                                    
                                }
                                else
                                {
                                    monHoc TU = new monHoc();
                                    TU.maMon = a.maMon;
                                    TU.tenMon = a.tenMon;
                                    TU.tinChi = a.tinChi;
                                  
                                    model.monHocs.Add(TU);
                                    model.SaveChanges();
                                }
                             
                               

                                //
                                //
                                //                             
                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.maMon == "" || a.maMon == null) data.Add("<li> Mã môn học is required</li>");
                                if (a.tenMon == "" || a.tenMon == null) data.Add("<li> Tên môn học is required</li>");
                                if (a.tinChi.ToString() == "" || a.tinChi.ToString() == null) data.Add("<li> Số tín chỉ is required</li>");
                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }
                    // LỚP HỌC
                    //
                    //

                    var lophoc = from a in excelFile.Worksheet<lopHoc>(sheetName) select a;
                    foreach (var a in lophoc)
                    {
                        try
                        {
                            if (a.maLop != "")
                            {
                                //
                                //
                                //
                                //Tạo hàm kiểm tra có tồn tại hay chưa 
                                var tontai = model.lopHocs.FirstOrDefault(x => x.maLop == a.maLop);
                                if (model.lopHocs.ToList().Any(o => o.maLop == a.maLop ))
                                {

                                }
                                else
                                {
                                    lopHoc TU = new lopHoc();
                                    TU.maLop = a.maLop;
                                   
                                    model.lopHocs.Add(TU);
                                    model.SaveChanges();
                                }

                                //
                                //
                                //       




                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.maLop == "" || a.maLop == null) data.Add("<li> Mã lớp is required</li>");

                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }

                    // TIẾT HỌC
                    //
                    //

                    var tiethoc = from a in excelFile.Worksheet<tietHoc>(sheetName) select a;
                    foreach (var a in tiethoc)
                    {
                        try
                        {
                            if (a.tongTiet.ToString() != "" && a.soTiet.ToString() != "" && a.tietHoc1.ToString() != "" && a.tietS.ToString() != "" && a.tietBD.ToString() != ""
                                )
                            {
                                // Nếu các trường thông tin của Tiết Học đã tồn tại thì skip qua
                                // Còn thiếu 1 trong các trường thông tin dưới mà thay đổi thì add vô
                                if (model.tietHocs.ToList().Any(o => o.tongTiet == a.tongTiet && o.soTiet == a.soTiet && o.tietHoc1 == a.tietHoc1 && o.tietS == a.tietS && o.tietBD == a.tietBD))
                                {

                                }
                                else
                                {
                                    tietHoc TU = new tietHoc();
                                    TU.tongTiet = a.tongTiet;
                                    TU.soTiet = a.soTiet;
                                    TU.tietHoc1 = a.tietHoc1;
                                    TU.tietS = a.tietS;
                                    TU.tietBD = a.tietBD;
                                  
                                    model.tietHocs.Add(TU);
                                    model.SaveChanges();
                                }
                                
                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.tongTiet.ToString() == "" || a.tongTiet.ToString() == null) data.Add("<li> Số tiết đã xếp is required</li>");
                                if (a.soTiet.ToString() == "" || a.soTiet.ToString() == null) data.Add("<li> Số tiết is required</li>");
                                if (a.tietHoc1.ToString() == "" || a.tietHoc1.ToString() == null) data.Add("<li> Tiết học is required</li>");
                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }

                    // PHÒNG HỌC
                    //
                    //

                    var phonghoc = from a in excelFile.Worksheet<phongHoc>(sheetName) select a;
                    foreach (var a in phonghoc)
                    {
                        try
                        {
                            if (a.loaiPhong != "" && a.sucChua.ToString() != "" && a.siSo.ToString() != "" && a.trong.ToString() != "" && a.soSVDK.ToString() != "" && a.maPhong != ""
                                )
                            {
                                // Nếu những trường thông tin ở Phòng Học giống nhau thì skip qua
                                // Nếu 1 trong những thông tin của Phòng Học thay đổi thì add vào
                                if (model.phongHocs.ToList().Any(o => o.loaiPhong == a.loaiPhong && o.siSo == a.siSo && o.trong == a.trong && o.soSVDK == a.soSVDK && o.maPhong == a.maPhong))
                                {

                                }
                                else
                                {
                                    phongHoc TU = new phongHoc();

                                    TU.loaiPhong = a.loaiPhong;
                                    TU.sucChua = a.sucChua;
                                    TU.siSo = a.siSo;
                                    TU.trong = a.trong;
                                    TU.soSVDK = a.soSVDK;
                                    TU.maPhong = a.maPhong;
                                
                                    model.phongHocs.Add(TU);
                                    model.SaveChanges();
                                }




                                
                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.loaiPhong == "" || a.loaiPhong == null) data.Add("<li> Loại Phòng is required</li>");
                                if (a.sucChua.ToString() == "" || a.sucChua.ToString() == null) data.Add("<li> Sức chứa is required</li>");
                                if (a.siSo.ToString() == "" || a.siSo.ToString() == null) data.Add("<li> Sỉ số is required</li>");
                                if (a.trong.ToString() == "" || a.trong.ToString() == null) data.Add("<li> Số lượng trống is required</li>");
                                if (a.soSVDK.ToString() == "" || a.soSVDK.ToString() == null) data.Add("<li> Số sinh viên đăng kí is required</li>");
                                if (a.maPhong == "" || a.maPhong == null) data.Add("<li> Mã phòng is required</li>");
                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }

                    // NGÀNH
                    //
                    //

                    var nganhhoc = from a in excelFile.Worksheet<Nganh>(sheetName) select a;
                    foreach (var a in nganhhoc)
                    {
                        try
                        {
                            if (a.maNganh.ToString() != "" && a.tenNganh != ""
                                )
                            {
                                // Nếu mã ngành và tên ngành đã tồn tại thì skip qua
                                // Nếu mã ngành hoặc tên ngành thay đổi thì add vào

                                if (model.Nganhs.ToList().Any(o => o.maNganh == a.maNganh && o.tenNganh == a.tenNganh ))
                                {

                                }
                                else
                                {
                                    Nganh TU = new Nganh();

                                    TU.maNganh = a.maNganh;
                                    TU.tenNganh = a.tenNganh;
                                    
                                    model.Nganhs.Add(TU);
                                    model.SaveChanges();
                                }
                                
                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.maNganh.ToString() == "" || a.maNganh.ToString() == null) data.Add("<li> Mã Ngành is required</li>");
                                if (a.tenNganh == "" || a.tenNganh.ToString() == null) data.Add("<li> Tên Ngành is required</li>");

                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }

                    // TUẦN HỌC
                    //
                    //

                    var tuanhoc = from a in excelFile.Worksheet<tuanHoc>(sheetName) select a;
                    foreach (var a in tuanhoc)
                    {
                        try
                        {
                            if (a.thuS.ToString() != "" && a.tuanBD.ToString() != "" && a.tuanHoc1.ToString() != "" && a.tuanKT.ToString() != "" && a.thu != ""
                                )
                            {
                                // Nếu các thông tin trong tuần học đã tồn tại thì skip qua 
                                if (model.tuanHocs.ToList().Any(o => o.thuS == a.thuS && o.tuanBD == a.tuanBD && o.tuanHoc1 == a.tuanHoc1 && o.tuanKT == a.tuanKT && o.thu == a.thu))
                                {

                                }
                                else
                                {
                                    tuanHoc TU = new tuanHoc();

                                    TU.thuS = a.thuS;
                                    TU.tuanBD = a.tuanBD;
                                    TU.tuanHoc1 = a.tuanHoc1;
                                    TU.tuanKT = a.tuanKT;
                                    TU.thu = a.thu;
                             
                                    model.tuanHocs.Add(TU);
                                    model.SaveChanges();
                                }

                                
                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.thuS.ToString() == "" || a.thuS.ToString() == null) data.Add("<li> Thứ bắt đầu is required</li>");
                                if (a.tuanBD.ToString() == "" || a.tuanBD.ToString() == null) data.Add("<li> Tuần bắt đầu is required</li>");
                                if (a.tuanHoc1.ToString() == "" || a.tuanHoc1.ToString() == null) data.Add("<li> Tuần học is required</li>");
                                if (a.tuanKT.ToString() == "" || a.tuanKT.ToString() == null) data.Add("<li> Tuần kiểm tra is required</li>");
                                if (a.thu == "" || a.thu == null) data.Add("<li> Thứ is required</li>");

                                data.Add("</ul>");
                                data.ToArray();
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }
                        }
                    }

                    // Giảng viên
                    //
                    //

                    // KHÔNG HOẠT ĐỘNG ĐƯỢC.


                    /*  var giangvien = from a in excelFile.Worksheet<nguoiDung>(sheetName) select a;
                      foreach (var a in giangvien)
                      {
                          try
                          {
                              // nếu mã giảng viên và tên giảng viên khác null
                              if (a.maGV != "" && a.tenGV != ""
                                  )



                                  if (model.nguoiDungs.ToList().Any(o => o.maGV == a.maGV || o.tenGV == a.tenGV ))
                                  {
                                      test.ID_nguoiDung = tontai.ID;
                                  }
                                  else
                                  {
                                      tuanHoc TU = new tuanHoc();

                                      TU.thuS = a.thuS;
                                      TU.tuanBD = a.tuanBD;
                                      TU.tuanHoc1 = a.tuanHoc1;
                                      TU.tuanKT = a.tuanKT;
                                      TU.thu = a.thu;
                                      test.ID = TU.ID;
                                      model.tuanHocs.Add(TU);
                                      model.SaveChanges();
                                  }
                              // thì thực hiện việc so sánh 
                              {
                                  var tontai = model.nguoiDungs.FirstOrDefault(x => x.maGV == a.maGV);
                                  var tontai1 = model.nguoiDungs.FirstOrDefault(x => x.tenGV == a.tenGV);
                                  // nếu hệ thống có tồn tại trường thông tin của giảng viên giống như trong file excel thì lưu ID vô khóa ngoại
                                  if (tontai != null || tontai1 != null)
                                  {


                                  }


                              }

                          }
                          catch (DbEntityValidationException ex)
                          {
                              foreach (var entityValidationErrors in ex.EntityValidationErrors)
                              {
                                  foreach (var validationError in entityValidationErrors.ValidationErrors)
                                  {
                                      Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                  }
                              }
                          }
                      }*/

                    //
                    //
                    //deleting excel file from folder
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    //
                    //
                    //

                   
                    model.SaveChanges();
                    return Json("Lưu Thành công", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //nếu định dạng khác template
                   
                    data.Add("Vui lòng chọn File Excel theo mẫu ở trên");
                    
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
               
                if (FileUpload == null) data.Add("Vui lòng Upload File Excel");
                
             
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    


        public ActionResult Search()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}