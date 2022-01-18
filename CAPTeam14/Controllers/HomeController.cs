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
            // nhớ sửa lại đường dẫn khi publish server
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
                    // nhớ sửa lại đường dẫn khi public server
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
                            if ((a.maGocLHP == null || a.maGocLHP == "") || (a.maLHP == null || a.maLHP == "") || (a.loaiHP == null || a.loaiHP == "") || (a.tinhtrangLHP == null || a.tinhtrangLHP == "") || (a.TSMH.ToString() == null || a.TSMH.ToString() == "")
                                )

                            {
                                if (a.maGocLHP == "" || a.maGocLHP == null) data.Add("Dữ liệu Mã gốc LHP đang bị thiếu");
                                if (a.maLHP == "" || a.maLHP == null) data.Add("Dữ liệu Mã LHP đang bị thiếu");
                                if (a.loaiHP == "" || a.loaiHP == null) data.Add("Dữ liệu Loại Học Phần đang bị thiếu");
                                if (a.tinhtrangLHP == "" || a.tinhtrangLHP == null) data.Add("Dữ liệu Tình trạng liên học phần đang bị thiếu");
                                if (a.TSMH.ToString() == "" || a.TSMH.ToString() == null) data.Add("Dữ liệu Tổng số môn học đang bị thiếu");


                                return Json(data, JsonRequestBehavior.AllowGet);
                                //
                                //

                                //Nếu mã gốc liên học phần đã tồn tại, thì bỏ qua 

                                


                                //
                                //
                                //

                            }
                            else
                            {
                                if (model.hocPhans.ToList().Any(o => o.maGocLHP == a.maGocLHP && o.maLHP == a.maLHP))
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
                            if ((a.maMon == null || a.maMon == "") || (a.tenMon == null || a.tenMon == "") || (a.tinChi.ToString() == null || a.tinChi.ToString() == "")
                                )
                            {

                                if (a.maMon == "" || a.maMon == null) data.Add("Dữ liệu Mã môn học đang bị thiếu");
                                if (a.tenMon == "" || a.tenMon == null) data.Add("Dữ liệu Tên môn học đang bị thiếu");
                                if (a.tinChi.ToString() == "" || a.tinChi.ToString() == null) data.Add("Dữ liệu Số tín chỉ đang bị thiếu");

                                return Json(data, JsonRequestBehavior.AllowGet);
                                



                                //
                                //
                                //                             
                            }
                            else
                            {

                                //
                                //
                                //
                                //Tạo hàm kiểm tra có tồn tại hay chưa 




                                // Nếu mã môn VÀ tên môn tồn tại tồn tại thì skip
                                // Hoặc nếu mã môn hoặc tên môn khác nhau thì add vô
                                // bây giờ là 00:24 ngày 11/01/2022. LTC vừa mất 4 tiếng cuộc đời
                                // Chỉ đê viết ra 14 dòng lệnh thỏa test case

                                if (model.monHocs.ToList().Any(o => o.maMon == a.maMon && o.tenMon == a.tenMon))
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
                            if ((a.maLop == null || a.maLop == ""))
                            {
                                if (a.maLop == "" || a.maLop == null) data.Add("Dữ liệu Mã lớp đang bị thiếu");



                                return Json(data, JsonRequestBehavior.AllowGet);
                                    




                            }
                            else
                            {
                                //
                                //
                                //
                                //Tạo hàm kiểm tra có tồn tại hay chưa 


                                //
                                //
                                //   
                                var tontai = model.lopHocs.FirstOrDefault(x => x.maLop == a.maLop);
                                if (model.lopHocs.ToList().Any(o => o.maLop == a.maLop))
                                {

                                }
                                else
                                {
                                    lopHoc TU = new lopHoc();
                                    TU.maLop = a.maLop;

                                    model.lopHocs.Add(TU);
                                    model.SaveChanges();
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
                    }

                    // TIẾT HỌC
                    //
                    //

                    var tiethoc = from a in excelFile.Worksheet<tietHoc>(sheetName) select a;
                    foreach (var a in tiethoc)
                    {
                        try
                        {
                            if ((a.tongTiet == null) || (a.soTiet == null) || (a.tietHoc1 == "" || a.tietHoc1 == null) || (a.tietS == null) && (a.tietBD == null)
                                )
                            {
                                if (a.tongTiet.ToString() == null) data.Add("Dữ liệu Số tiết đã xếp đang bị thiếu");
                                if (a.soTiet.ToString() == null) data.Add("Dữ liệu Số tiết đang bị thiếu");
                                if (a.tietHoc1 == null) data.Add("Dữ liệu Tiết học đang bị thiếu");
                                if (a.tietS.ToString() == null) data.Add("Dữ liệu TiếtS đang bị thiếu");
                                if (a.tietBD.ToString() == null) data.Add("Dữ liệu Tiết bắt đầu đang bị thiếu");
                                return Json(data, JsonRequestBehavior.AllowGet);

                                

                            }
                            else
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
                            if ((a.loaiPhong == "" || a.loaiPhong == null) || (a.sucChua.ToString() == "" || a.sucChua.ToString() == null) || (a.siSo.ToString() == "" || a.siSo.ToString() == null) || (a.trong.ToString() == "" || a.trong.ToString() == null) || (a.soSVDK.ToString() == "" || a.soSVDK.ToString() == null) || (a.maPhong == "" || a.maPhong == null)
                                )
                            {
                                if (a.loaiPhong == "" || a.loaiPhong == null) data.Add("Dữ liệu Loại Phòng đang bị thiếu");
                                if (a.sucChua.ToString() == "" || a.sucChua.ToString() == null) data.Add("Dữ liệu Sức chứa đang bị thiếu");
                                if (a.siSo.ToString() == "" || a.siSo.ToString() == null) data.Add("Dữ liệu Sỉ số đang bị thiếu");
                                if (a.trong.ToString() == "" || a.trong.ToString() == null) data.Add("Dữ liệu Số lượng trống đang bị thiếu");
                                if (a.soSVDK.ToString() == "" || a.soSVDK.ToString() == null) data.Add("Dữ liệu Số sinh viên đăng kí đang bị thiếu");
                                if (a.maPhong == "" || a.maPhong == null) data.Add("Dữ liệu Mã phòng đang bị thiếu");

                                return Json(data, JsonRequestBehavior.AllowGet);
                                





                            }
                            else
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
                            if ((a.maNganh.ToString() == "" || a.maNganh.ToString() == null) || (a.tenNganh == "" || a.tenNganh == null)
                                )
                            {
                                if (a.maNganh.ToString() == "" || a.maNganh.ToString() == null) data.Add("Dữ liệu Mã Ngành đang bị thiếu");
                                if (a.tenNganh == "" || a.tenNganh.ToString() == null) data.Add("Dữ liệu Tên Ngành đang bị thiếu");


                                return Json(data, JsonRequestBehavior.AllowGet);
                                
                            }
                            else
                            {

                                // Nếu mã ngành và tên ngành đã tồn tại thì skip qua
                                // Nếu mã ngành hoặc tên ngành thay đổi thì add vào

                                if (model.Nganhs.ToList().Any(o => o.maNganh == a.maNganh && o.tenNganh == a.tenNganh))
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
                            if ((a.thuS.ToString() == "" || a.thuS.ToString() == null) || (a.tuanBD.ToString() == "" || a.tuanBD.ToString() == null) || (a.tuanHoc1.ToString() == "" || a.tuanHoc1.ToString() == null) || (a.tuanKT.ToString() == "" || a.tuanKT.ToString() == null) || (a.thu == "" || a.thu == null)
                                )
                            {

                                if (a.thuS.ToString() == "" || a.thuS.ToString() == null) data.Add("Dữ liệu Thứ bắt đầu đang bị thiếu");
                                if (a.tuanBD.ToString() == "" || a.tuanBD.ToString() == null) data.Add("Dữ liệu Tuần bắt đầu đang bị thiếu");
                                if (a.tuanHoc1.ToString() == "" || a.tuanHoc1.ToString() == null) data.Add("Dữ liệu Tuần học đang bị thiếu");
                                if (a.tuanKT.ToString() == "" || a.tuanKT.ToString() == null) data.Add("Dữ liệu Tuần kiểm tra đang bị thiếu");
                                if (a.thu == "" || a.thu == null) data.Add("Dữ liệu Thứ (bằng chữ) đang bị thiếu");

                                return Json(data, JsonRequestBehavior.AllowGet);
                                

                            }
                            else
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