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
using ExcelDataReader;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;


// 02/03/2022 Cập nhật lại luồng đi Import, cải thiện tốc độ performance bằng cách chuyển hết định dạng sang chuỗi
// 02/03/22 04:42 đang dừng lại ở câu lệnh kiểm tra học phần đã tồn tại hay chưa. Đã Cập nhật database và thêm table Học Kỳ
// 03/03/22 3:59 đã hoành thiện chức năng import dữ liệu kiểu mới. Tuy nhiên cần cải thiện lại về giao diện và cần thêm/sửa học kì cũng như lấy
// dữ liệu của học kì vào thời khóa biểu tổng





namespace CAPTeam14.Controllers
{
    [LoginVerification]
    public class HomeController : Controller
    {
        CP24Team14Entities model = new CP24Team14Entities();

        // hiển thị danh sách học kì
        // khi chọn danh sách học kì sẽ hiển thị thời khóa biểu với học kì tương ứng
        public ActionResult Index1()
        {
            var tkb = model.hocKies.OrderBy(x => x.ID).ToList();
            var test = model.TKBs.OrderByDescending(x => x.ID).Count();
            ViewBag.test = test;
            
            
            return View(tkb);
        }

        public ActionResult Index2(int? id, string selectedId)
        {
            if (selectedId == null)
            {
                selectedId = "0";
            }
            else if (selectedId != null)
            {
                TempData["tuan"] = selectedId;
            }
            /*TempData["ketquatuan"] = "0";
            foreach (var tuan in model.TKBs.Where(x => x.ID_hocKy == id).Select(x => x.tuanHoc.tuanHoc1).Distinct())
            {
                string cc = tuan;
                string[] cl = cc.Split(',', ';', ' ');
                TempData["Tuan1"] = "0";

                foreach (var clm in cl)
                {
                    string dmm = clm;
                    if (dmm == selectedId)
                    {
                        TempData["Tuan1"] = selectedId;
                        break;
                    }




                }
                break;




            };
            //
            foreach (var tuan2 in model.TKBs.Where(x => x.ID_hocKy == id).Select(x => x.tuanHoc.tuanHoc1).Distinct())
            {
                string cc1 = tuan2;
                string[] cl1 = cc1.Split(',', ';', ' ');
                TempData["Tuan2"] = "0";

                foreach (var clm1 in cl1)
                {
                    string dmm1 = clm1;
                    if (dmm1 == selectedId)
                    {
                        TempData["Tuan2"] = selectedId;
                        break;
                    }




                }





            };

            if ((string)TempData["Tuan1"] == selectedId)
            {
                TempData["ketquatuan"] = (string)TempData["Tuan1"];
            }
            else
            {
                if ((string)TempData["Tuan2"] == selectedId)
                {
                    TempData["ketquatuan"] = (string)TempData["Tuan2"];
                }
            }*/
            //
           
            //


            //ViewBag.IDs = new SelectList(model.tuans.OrderBy(x => x.ID), "", "ID");
            ViewBag.hkyy = model.tuans.OrderBy(x => x.ID).ToList();

            // lấy danh sách thời khóa biểu
            var tkb = model.TKBs.OrderByDescending(x => x.ID).ToList();
            var tkb12 = model.TKBs.FirstOrDefault(x => x.ID_hocKy == id);
            // lấy danh sách học kì
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            //hiển thị tên học kì đã chọn
            ViewBag.test2 = tkb1.tenHK;
            ViewBag.test3 = tkb1.ID;
            TempData["test"] = tkb1.ID;
           
            
         
           

            // lấy thông tin ID học kì đã chọn trong thời khóa biểu
            ViewBag.test = id;

            ViewBag.gv = model.nguoiDungs.Where(x => x.role == 4).OrderByDescending(x => x.ID).ToList();
            return View(tkb);
        }


        [HttpGet]
        public ActionResult Index(int? id, string selectedId)
        {
            if (selectedId == null)
            {
                selectedId = "0";
            }
            else if (selectedId != null)
            {
                TempData["tuan"] = selectedId;
            }    
            
            TempData["ketquatuan"] = "0";
            /*foreach (var tuan in model.TKBs.Where(x => x.ID_hocKy == id).Select(x => x.tuanHoc.tuanHoc1).Distinct().Contains(selectedId))
            {
                string cc = tuan;
                string[] cl = cc.Split(',', ';', ' ');
                TempData["Tuan1"] = "0";

                foreach (var clm in cl)
                {
                    string dmm = clm;
                    if (dmm == selectedId)
                    {
                        TempData["Tuan1"] = selectedId;
                        break;
                    }
                    

                   

                }
                break;




            };
            //
            foreach (var tuan2 in model.TKBs.Where(x => x.ID_hocKy == id).Select(x => x.tuanHoc.tuanHoc1).Distinct())
            {
                string cc1 = tuan2;
                string[] cl1 = cc1.Split(',', ';', ' ');
                TempData["Tuan2"] = "0";

                foreach (var clm1 in cl1)
                {
                    string dmm1 = clm1;
                    if (dmm1 == selectedId)
                    {
                        TempData["Tuan2"] = selectedId;
                        break;
                    }




                }
               




            };

            if ((string)TempData["Tuan1"] == selectedId)
            {
                TempData["ketquatuan"] = (string)TempData["Tuan1"];
            }
            else
            {
                if ((string)TempData["Tuan2"] == selectedId )
                {
                    TempData["ketquatuan"] = (string)TempData["Tuan2"];
                }
            }*/
            //
         
            //


            //ViewBag.IDs = new SelectList( model.tuans.OrderBy(x => x.ID), "ID", "sotuan");
            ViewBag.hkyy = model.tuans.OrderBy(x => x.ID).ToList();
          
            // lấy danh sách thời khóa biểu
            var tkb = model.TKBs.OrderBy(x => x.ID).ToList();
            // lấy danh sách học kì
                var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            //hiển thị tên học kì đã chọn
            ViewBag.test2 = tkb1.tenHK;
            ViewBag.test3 = tkb1.ID;
            TempData["test"] = tkb1.ID;
            if (tkb1.lockstat == false || tkb1.lockstat == null)
            {
                ViewBag.lockstat = 2;
            }
            else
            {
                ViewBag.lockstat = 1;
            }
            // lấy thông tin ID học kì đã chọn trong thời khóa biểu
            ViewBag.test = id;

            ViewBag.gv = model.danhsachGVs.OrderBy(x => x.ID).ToList();
            return View(tkb);
        }
        [HttpGet]
        public ActionResult IndexEx(int? id)
        {

            // lấy danh sách thời khóa biểu
            var tkb = model.TKBs.OrderBy(x => x.ID).ToList();
            // lấy danh sách học kì
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            //hiển thị tên học kì đã chọn
            ViewBag.test2 = tkb1.tenHK;
            ViewBag.test3 = tkb1.ID;
            TempData["test"] = tkb1.ID;
            // lấy thông tin ID học kì đã chọn trong thời khóa biểu
            ViewBag.test = id;
            // xuất thời khóa biểu

            ViewBag.ex =

            ViewBag.gv = model.danhsachGVs.OrderBy(x => x.ID).ToList();
            return View(tkb);
        }
        // Query Delete TKB
        public ActionResult DeleteTKB(int? id)
        {
            ViewBag.test = model.TKBs.Where(x => x.ID_hocKy == id).OrderByDescending(x => x.ID).ToList();
            var query = model.Database.SqlQuery<int>("SELECT ID_GV FROM [TKB] WHERE ID_hocKy = @ID_hocKy AND ID_GV IS NOT NULL ", new SqlParameter("@ID_hocKy", id)).ToList().Count();
            if (query == 0)
            {
                model.Database.ExecuteSqlCommand("DELETE FROM [TKB] WHERE ID_hocKy = @ID_hocKy ", new SqlParameter("@ID_hocKy", id));

                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else if (query != 0)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        // Luồng đi mới của Import
        public ActionResult Catalog(int? id)
        {
            //

            //

            ViewBag.hk1 = model.hocKies.OrderBy(x => x.ID).ToList();
            ViewBag.tenlop = model.lopHocs.OrderByDescending(x => x.ID).ToList();
            ViewBag.tennganh = model.Nganhs.OrderByDescending(x => x.ID).ToList();
            //
            ViewBag.hocky = model.hocKies.OrderByDescending(x => x.ID).ToList();
            // lấy id của học kì đã chọn
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            // hiển thị tên học kì đã chọn
            Session["test2"] = tkb1.tenHK;
            ViewBag.test1 = id;

            Session["nambd"] = tkb1.namBD;
            Session["namkt"] = tkb1.namKT;
            /*  ViewBag.tenlop = tkb1.lopHoc.maLop;
              ViewBag.nganh = tkb1.Nganh.tenNganh;*/
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
            string path = "/CP24Team14/Doc/Template.xlsx";
            return File(path, "application/vnd.ms-excel", "TemplateTKB.xlsx");
        }

        public ActionResult Catalog(TKB tkb, int? id, hocKy hk)
        {
            try
            {
                ViewBag.tenlop = model.lopHocs.OrderByDescending(x => x.ID).ToList();
                ViewBag.tennganh = model.Nganhs.OrderByDescending(x => x.ID).ToList();
                var hocky1 = model.hocKies.FirstOrDefault(x => x.ID == id);
                hocky1.ID_nganh = hk.ID_nganh;
                hocky1.ID_lop = hk.ID_lop;
                model.SaveChanges();


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
                        var hocphan = new hocPhan(); // 1
                        var lop = new lopHoc(); // 2
                        var mon = new monHoc(); // 3 
                        var nganh = new Nganh(); // 4
                        var phong = new phongHoc(); // 5
                        var tiet = new tietHoc(); // 6
                        var tuan = new tuanHoc(); // 7
                        var tkbTong = new TKB(); // 8
                        var hocki = new hocKy();
                        var giangvien = new danhsachGV();





                        // Dữ liệu từng cột theo thứ tự

                        String magocLHP = cot[0].ToString();
                        String maMon = cot[1].ToString();
                        String maLHP = cot[2].ToString();
                        String tenMon = cot[3].ToString();
                        String soTC = cot[4].ToString();
                        String loaiHP = cot[5].ToString();
                        String maLop = cot[6].ToString();
                        String TSMH = cot[7].ToString();
                        String tongTiet = cot[8].ToString();
                        String phTrong = cot[9].ToString(); // không dùng
                        String thu = cot[10].ToString();
                        String tietBD = cot[11].ToString();
                        String soTiet = cot[12].ToString();
                        String tietHoc = cot[13].ToString();
                        String phongHoc = cot[14].ToString();
                        String maGV = cot[15].ToString();
                        String tenGV = cot[16].ToString();
                        String PHX = cot[17].ToString();
                        String sucChua = cot[18].ToString();
                        String siSo = cot[19].ToString();
                        String Trong = cot[20].ToString();
                        String tinhtrangLHP = cot[21].ToString();
                        String tuanHoc = cot[22].ToString();
                        String thuS = cot[23].ToString();
                        String tietS = cot[24].ToString();
                        String soSVDK = cot[25].ToString();
                        String tuanBD = cot[26].ToString();
                        String tuanKT = cot[27].ToString();
                        String maNganh = cot[28].ToString();
                        String tenNganh = cot[29].ToString();

                        //Không có cột ghi chú

                        //Kiểm Tra xem dữ liệu đã tồn tại hay chưa
                        var checkgv = model.danhsachGVs.FirstOrDefault(x => x.maGV == maGV && x.tenGV == tenGV); // new


                        // 30 - 3 = 27 

                        // table hocPhans
                        var checkhocphan = model.hocPhans.FirstOrDefault(x => x.maGocLHP == magocLHP && x.maLHP == maLHP && x.loaiHP == loaiHP && x.tinhtrangLHP == tinhtrangLHP && x.TSMH == TSMH); // 1


                        // table lopHocs
                        var checklopHoc = model.lopHocs.FirstOrDefault(x => x.maLop == maLop); // 6

                        // table tuanHoc
                        var checktuanhoc = model.tuanHocs.FirstOrDefault(x => x.thuS == thuS && x.tuanBD == tuanBD && x.tuanHoc1 == tuanHoc && x.tuanKT == tuanKT && x.thu == thu); // 7


                        // table Nganh
                        var checkmaNganh = model.Nganhs.FirstOrDefault(x => x.maNganh == maNganh && x.tenNganh == tenNganh); // 12


                        // table tietHocs
                        var checktiethoc = model.tietHocs.FirstOrDefault(x => x.tongTiet == tongTiet && x.soTiet == soTiet && x.tietHoc1 == tietHoc && x.tietS == tietS && x.tietBD == tietBD); // 14


                        // table monHoc
                        var checkmonhoc = model.monHocs.FirstOrDefault(x => x.maMon == maMon && x.tenMon == tenMon && x.tinChi == soTC); // 19


                        // table phongHoc
                        var checkphong = model.phongHocs.FirstOrDefault(x => x.loaiPhong == PHX && x.sucChua == sucChua && x.siSo == siSo && x.trong == Trong && x.soSVDK == soSVDK && x.maPhong == phongHoc); // 22



                        //new
                        if (checkgv == null)
                        {
                            if (tenGV != "" && maGV != "")
                            {
                                giangvien = new danhsachGV
                                {
                                    tenGV = tenGV,
                                    maGV = maGV,
                                };
                                model.danhsachGVs.Add(giangvien);
                                model.SaveChanges();
                                tkbTong.ID_GV = giangvien.ID;
                            }


                        }
                        else
                        {
                            giangvien = checkgv;

                            tkbTong.ID_GV = checkgv.ID;


                        }

                        // Học phần

                        // câu lệnh kiểm tra xem học phần đã tồn tại hay chưa
                        // nếu chưa tồn tại thì tạo mới Học Phần
                        if (checkhocphan == null)
                        {
                            if ((magocLHP != "") && (maLHP != "") && (loaiHP != "") &&
                               (tinhtrangLHP != "") && (TSMH != ""))
                            {
                                hocphan = new hocPhan
                                {
                                    maGocLHP = magocLHP,
                                    maLHP = maLHP,
                                    loaiHP = loaiHP,
                                    tinhtrangLHP = tinhtrangLHP,
                                    TSMH = TSMH,
                                };
                                model.hocPhans.Add(hocphan);
                                model.SaveChanges();

                                tkbTong.ID_hocPhan = hocphan.ID;
                            }
                        }
                        // còn nếu đã tồn tại một trong những dữ liệu trên rồi thì lưu giữ dữ liệu đã tồn tại và tiếp tục vòng lặp để kiểm tra 
                        // xem có dữ liệu nào khác thì
                        else
                        {
                            hocphan = checkhocphan;

                            tkbTong.ID_hocPhan = checkhocphan.ID;

                        }

                        // lớp học
                        if (checklopHoc == null)
                        {
                            if (maLop != "")
                            {
                                lop = new lopHoc
                                {
                                    maLop = maLop
                                };
                                model.lopHocs.Add(lop);
                                model.SaveChanges();

                                tkbTong.ID_Lop = lop.ID;
                            }


                        }
                        //
                        else
                        {
                            lop = checklopHoc;
                            tkbTong.ID_Lop = checklopHoc.ID;
                        }

                        // tuần học
                        if (checktuanhoc == null)
                        {
                            if ((thuS != "") && (tuanBD != "") && (tuanHoc != "") && (tuanKT != "")
                              && (thu != ""))
                            {
                                tuan = new tuanHoc
                                {
                                    thuS = thuS,
                                    tuanBD = tuanBD,
                                    tuanHoc1 = tuanHoc,
                                    tuanKT = tuanKT,
                                    thu = thu,
                                };
                                model.tuanHocs.Add(tuan);
                                model.SaveChanges();

                                tkbTong.ID_Tuan = tuan.ID;

                            }

                        }
                        //
                        else
                        {
                            tuan = checktuanhoc;


                            tkbTong.ID_Tuan = checktuanhoc.ID;

                        }

                        // ngành
                        if (checkmaNganh == null)
                        {
                            if ((maNganh != "") && (tenNganh != ""))
                            {
                                nganh = new Nganh
                                {
                                    maNganh = maNganh,
                                    tenNganh = tenNganh,
                                };
                                model.Nganhs.Add(nganh);
                                model.SaveChanges();

                                tkbTong.ID_Nganh = nganh.ID;
                            }


                        }
                        //
                        else
                        {
                            nganh = checkmaNganh;

                            tkbTong.ID_Nganh = checkmaNganh.ID;

                        }

                        // tiết học
                        if (checktiethoc == null)
                        {
                            if ((tongTiet != "") && (soTiet != "") && (tietHoc != "")
                                && (tietS != "") && (tietBD != ""))
                            {
                                tiet = new tietHoc
                                {
                                    tongTiet = tongTiet,
                                    soTiet = soTiet,
                                    tietHoc1 = tietHoc,
                                    tietS = tietS,
                                    tietBD = tietBD,
                                };
                                model.tietHocs.Add(tiet);
                                model.SaveChanges();

                                tkbTong.ID_Tiet = tiet.ID;
                            }


                        }
                        //
                        else
                        {
                            tiet = checktiethoc;

                            tkbTong.ID_Tiet = checktiethoc.ID;
                        }

                        // môn học
                        if (checkmonhoc == null)
                        {
                            if ((maMon != "") && (tenMon != "") && (soTC != ""))
                            {
                                mon = new monHoc
                                {
                                    maMon = maMon,
                                    tenMon = tenMon,
                                    tinChi = soTC,
                                };
                                model.monHocs.Add(mon);
                                model.SaveChanges();

                                tkbTong.ID_monHoc = mon.ID;
                            }


                        }
                        //
                        else
                        {

                            mon = checkmonhoc;

                            tkbTong.ID_monHoc = checkmonhoc.ID;
                        }

                        // phòng học
                        //  checksucChua == null || checksiSo == null || checktrong == null || checksoSVDK == null ||
                        if (checkphong == null)
                        {
                            if ((PHX != "") && (sucChua != "") && (siSo != "") && (Trong != "") && (soSVDK != "")
                              && (phongHoc != ""))
                            {
                                phong = new phongHoc
                                {
                                    loaiPhong = PHX,
                                    sucChua = sucChua,
                                    siSo = siSo,
                                    trong = Trong,
                                    soSVDK = soSVDK,
                                    maPhong = phongHoc,
                                };
                                model.phongHocs.Add(phong);
                                model.SaveChanges();

                                tkbTong.ID_Phong = phong.ID;
                            }


                        }
                        //
                        else
                        {
                            phong = checkphong;

                            tkbTong.ID_Phong = checkphong.ID;
                        }
                        // kiểm tra dữ liệu thời khóa biểu đã tồn tại hay chưa
                        // table tkb

                        /* var checktkblop = model.TKBs.FirstOrDefault(x => x.ID_Lop == lop.ID);
                         var checktkbtuan = model.TKBs.FirstOrDefault(x => x.ID_Tuan == tuan.ID);
                         var checktkbnganh = model.TKBs.FirstOrDefault(x => x.ID_Nganh == nganh.ID);
                         var checktkbtiet = model.TKBs.FirstOrDefault(x => x.ID_Tiet == tiet.ID);
                         var checktkbmon = model.TKBs.FirstOrDefault(x => x.ID_monHoc == mon.ID);
                         var checktkbphong = model.TKBs.FirstOrDefault(x => x.ID_Phong == phong.ID);*/

                        //checktkbhp == null || checktkblop == null || checktkbtuan == null || checktkbnganh == null ||
                        // checktkbtiet == null || checktkbmon == null || checktkbphong == null



                        var checktkbhp = model.TKBs.Where(x => x.ID_hocPhan == tkbTong.ID_hocPhan).FirstOrDefault(a => a.ID_hocKy == id && a.ID_Lop == tkbTong.ID_Lop && a.ID_monHoc == tkbTong.ID_monHoc
                        && a.ID_Nganh == tkbTong.ID_Nganh && a.ID_Phong == tkbTong.ID_Phong && a.ID_Tiet == tkbTong.ID_Tiet && a.ID_Tuan == tkbTong.ID_Tuan);
                        var checkhk = model.TKBs.FirstOrDefault(x => x.ID_hocKy == id);

                        // nếu học phần chưa tồn tại => nếu học kì 
                        if (checktkbhp == null)
                        {
                            /*  if (checkhk == null)
                              {

                              }*/
                            tkbTong.ID_hocKy = id;
                            model.TKBs.Add(tkbTong);
                            model.SaveChanges();

                        }



                    }
                    break;
                }
                // kết thúc vòng lặp và ngưng đọc dữ liệu sau 29 cột
                IEDreader.Close();
                TempData["ThongBao1"] = 1;
                return RedirectToAction("Index", "Home", new { id = id });
              
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Không thể thực hiện hành động này, vui lòng kiểm tra File Excel có đúng định dạng và nội dung file Excel");
            }
            return View(tkb);



        }

        [HttpGet]
        // Luồng đi mới của Import
        public ActionResult Catalog1(int? id)
        {
            //

            //

            ViewBag.hk1 = model.hocKies.OrderBy(x => x.ID).ToList();
            ViewBag.tenlop = model.lopHocs.OrderByDescending(x => x.ID).ToList();
            ViewBag.tennganh = model.Nganhs.OrderByDescending(x => x.ID).ToList();
            //
            ViewBag.hocky = model.hocKies.OrderByDescending(x => x.ID).ToList();
            // lấy id của học kì đã chọn
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            // hiển thị tên học kì đã chọn
            Session["test2"] = tkb1.tenHK;
            ViewBag.test1 = id;

            Session["nambd"] = tkb1.namBD;
            Session["namkt"] = tkb1.namKT;
            /*  ViewBag.tenlop = tkb1.lopHoc.maLop;
              ViewBag.nganh = tkb1.Nganh.tenNganh;*/
            return View();
        }

        public ActionResult Catalog1(TKB tkb, int? id, hocKy hk, hocPhan ph, lopHoc l, monHoc monh, Nganh nga, phongHoc pho, tietHoc ti, tuanHoc tu, TKB t, danhsachGV ds)
        {

            try
            {
                ViewBag.tenlop = model.lopHocs.OrderByDescending(x => x.ID).ToList();
                ViewBag.tennganh = model.Nganhs.OrderByDescending(x => x.ID).ToList();
                var hocky1 = model.hocKies.FirstOrDefault(x => x.ID == id);
                hocky1.ID_nganh = hk.ID_nganh;
                hocky1.ID_lop = hk.ID_lop;
                model.SaveChanges();


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
                        var hocphan = new hocPhan(); // 1
                        var lop = new lopHoc(); // 2
                        var mon = new monHoc(); // 3 
                        var nganh = new Nganh(); // 4
                        var phong = new phongHoc(); // 5
                        var tiet = new tietHoc(); // 6
                        var tuan = new tuanHoc(); // 7
                        var tkbTong = new TKB(); // 8
                        var hocki = new hocKy();
                        var giangvien = new danhsachGV();





                        // Dữ liệu từng cột theo thứ tự

                        String magocLHP = cot[0].ToString();
                        String maMon = cot[1].ToString();
                        String maLHP = cot[2].ToString();
                        String tenMon = cot[3].ToString();
                        String soTC = cot[4].ToString();
                        String loaiHP = cot[5].ToString();
                        String maLop = cot[6].ToString();
                        String TSMH = cot[7].ToString();
                        String tongTiet = cot[8].ToString();
                        String phTrong = cot[9].ToString(); // không dùng
                        String thu = cot[10].ToString();
                        String tietBD = cot[11].ToString();
                        String soTiet = cot[12].ToString();
                        String tietHoc = cot[13].ToString();
                        String phongHoc = cot[14].ToString();
                        String maGV = cot[15].ToString();
                        String tenGV = cot[16].ToString();
                        String PHX = cot[17].ToString();
                        String sucChua = cot[18].ToString();
                        String siSo = cot[19].ToString();
                        String Trong = cot[20].ToString();
                        String tinhtrangLHP = cot[21].ToString();
                        String tuanHoc = cot[22].ToString();
                        String thuS = cot[23].ToString();
                        String tietS = cot[24].ToString();
                        String soSVDK = cot[25].ToString();
                        String tuanBD = cot[26].ToString();
                        String tuanKT = cot[27].ToString();
                        String maNganh = cot[28].ToString();
                        String tenNganh = cot[29].ToString();

                        //Không có cột ghi chú

                        //Kiểm Tra xem dữ liệu đã tồn tại hay chưa
                        var checkgv = model.danhsachGVs.FirstOrDefault(x => x.maGV == maGV && x.tenGV == tenGV); // new


                        // 30 - 3 = 27 

                        // table hocPhans
                        var checkhocphan = model.hocPhans.FirstOrDefault(x => x.maGocLHP == magocLHP && x.maLHP == maLHP && x.loaiHP == loaiHP && x.tinhtrangLHP == tinhtrangLHP && x.TSMH == TSMH); // 1


                        // table lopHocs
                        var checklopHoc = model.lopHocs.FirstOrDefault(x => x.maLop == maLop); // 6

                        // table tuanHoc
                        var checktuanhoc = model.tuanHocs.FirstOrDefault(x => x.thuS == thuS && x.tuanBD == tuanBD && x.tuanHoc1 == tuanHoc && x.tuanKT == tuanKT && x.thu == thu); // 7


                        // table Nganh
                        var checkmaNganh = model.Nganhs.FirstOrDefault(x => x.maNganh == maNganh && x.tenNganh == tenNganh); // 12


                        // table tietHocs
                        var checktiethoc = model.tietHocs.FirstOrDefault(x => x.tongTiet == tongTiet && x.soTiet == soTiet && x.tietHoc1 == tietHoc && x.tietS == tietS && x.tietBD == tietBD); // 14


                        // table monHoc
                        var checkmonhoc = model.monHocs.FirstOrDefault(x => x.maMon == maMon && x.tenMon == tenMon && x.tinChi == soTC); // 19


                        // table phongHoc
                        var checkphong = model.phongHocs.FirstOrDefault(x => x.loaiPhong == PHX && x.sucChua == sucChua && x.siSo == siSo && x.trong == Trong && x.soSVDK == soSVDK && x.maPhong == phongHoc); // 22



                        //new

                        if (checktiethoc == null)
                        {
                            if ((tongTiet != "") && (soTiet != "") && (tietHoc != "")
                                    && (tietS != "") && (tietBD != ""))
                            {
                                tiet = new tietHoc
                                {
                                    tongTiet = tongTiet,
                                    soTiet = soTiet,
                                    tietHoc1 = tietHoc,
                                    tietS = tietS,
                                    tietBD = tietBD,
                                };
                                model.tietHocs.Add(tiet);
                                model.SaveChanges();

                                tkbTong.ID_Tiet = tiet.ID;
                            }
                        }
                        else
                        {
                            tiet = checktiethoc;

                            tkbTong.ID_Tiet = checktiethoc.ID;
                        }
                        //
                        // tuần học
                        if (checktuanhoc == null)
                        {
                            if ((thuS != "") && (tuanBD != "") && (tuanHoc != "") && (tuanKT != "")
                              && (thu != ""))
                            {
                                tuan = new tuanHoc
                                {
                                    thuS = thuS,
                                    tuanBD = tuanBD,
                                    tuanHoc1 = tuanHoc,
                                    tuanKT = tuanKT,
                                    thu = thu,
                                };
                                model.tuanHocs.Add(tuan);
                                model.SaveChanges();

                                tkbTong.ID_Tuan = tuan.ID;

                            }

                        }
                        //
                        else
                        {
                            tuan = checktuanhoc;


                            tkbTong.ID_Tuan = checktuanhoc.ID;

                        }
                        //
                        // Học phần


                        if (checkhocphan != null)
                        {
                            hocphan = checkhocphan;

                            tkbTong.ID_hocPhan = checkhocphan.ID;
                        }

                        // lớp học
                        if (checklopHoc != null)
                        {
                            lop = checklopHoc;
                            tkbTong.ID_Lop = checklopHoc.ID;


                        }



                        // ngành
                        if (checkmaNganh != null)
                        {
                            nganh = checkmaNganh;

                            tkbTong.ID_Nganh = checkmaNganh.ID;


                        }
                        //




                        // môn học
                        if (checkmonhoc != null)
                        {
                            mon = checkmonhoc;

                            tkbTong.ID_monHoc = checkmonhoc.ID;


                        }

                        // phòng học
                        //  checksucChua == null || checksiSo == null || checktrong == null || checksoSVDK == null ||
                        if (checkphong != null)
                        {
                            phong = checkphong;

                            tkbTong.ID_Phong = checkphong.ID;


                        }
                        //




                        /*var edit = model.TKBs.Where(a => a.ID_Tiet != checktiethoc.ID || a.ID_Tuan != checktuanhoc.ID).Distinct().FirstOrDefault(x => x.ID_hocKy == id && x.ID_Lop == checklopHoc.ID && x.ID_monHoc == checkmonhoc.ID
                          && x.ID_Nganh == checkmaNganh.ID && x.ID_Phong == checkphong.ID && x.ID_hocPhan == checkhocphan.ID );
                        edit.ID_Tuan = checktuanhoc.ID;
                        edit.ID_Tiet = checktiethoc.ID;
                        model.SaveChanges();*/



                        var checktkbhp = model.TKBs.Distinct().FirstOrDefault(x => x.ID_hocKy == id && x.ID_Lop == tkbTong.ID_Lop && x.ID_monHoc == tkbTong.ID_monHoc
                         && x.ID_Nganh == tkbTong.ID_Nganh && x.ID_Phong == tkbTong.ID_Phong && x.ID_hocPhan == tkbTong.ID_hocPhan && x.ID_Tuan == tkbTong.ID_Tuan);

                        /*var checktkbhp1 = model.TKBs.Where(a => a.ID_Tuan != tkbTong.ID_Tuan).Distinct().FirstOrDefault(x => x.ID_hocKy == id && x.ID_Lop == tkbTong.ID_Lop && x.ID_monHoc == tkbTong.ID_monHoc
                          && x.ID_Nganh == tkbTong.ID_Nganh && x.ID_Phong == tkbTong.ID_Phong && x.ID_hocPhan == tkbTong.ID_hocPhan);

                        var checktkbhp2 = model.TKBs.Where(a => a.ID_Tuan != tkbTong.ID_Tuan && a.ID_Tiet != tkbTong.ID_Tiet).Distinct().FirstOrDefault(x => x.ID_hocKy == id && x.ID_Lop == tkbTong.ID_Lop && x.ID_monHoc == tkbTong.ID_monHoc
                         && x.ID_Nganh == tkbTong.ID_Nganh && x.ID_Phong == tkbTong.ID_Phong && x.ID_hocPhan == tkbTong.ID_hocPhan);*/

                        var checkhk = model.TKBs.FirstOrDefault(x => x.ID_hocKy == id);

                        //
                        if (checktkbhp != null)
                        {

                            checktkbhp.ID_Tiet = tkbTong.ID_Tiet;
                            model.SaveChanges();
                        }
                        //


                    }
                    break;
                }
                // kết thúc vòng lặp và ngưng đọc dữ liệu sau 29 cột
                IEDreader.Close();
                TempData["ThongBao2"] = 1;
                return RedirectToAction("Index", "Home", new { id = id });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Không thể thực hiện hành động này, vui lòng kiểm tra File Excel có đúng định dạng và nội dung file Excel");
            }
            return View(tkb);



        }


        public ActionResult Courses()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Test()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ActionResult PhanCong(int? id, TKB tkb, int? idGV)
        {
            ViewBag.gv = model.danhsachGVs.OrderByDescending(x => x.ID).ToList();

            var tt = model.TKBs.FirstOrDefault(x => x.ID == id);
            
            var gvid_warning = model.TKBs.Where(g => g.ID_GV == tkb.ID_GV && g.tuanHoc.thuS == tt.tuanHoc.thuS && g.ID_hocKy == tt.ID_hocKy).ToList().Count(g => g.ID_GV != null);
            if (gvid_warning >= 3)
            {
                
                tt.ID_GV = tkb.ID_GV;
                model.SaveChanges();
                var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID_dsGV == tt.ID_GV);
                if (giangvien != null)
                {
                    SendEmailToUser(giangvien.AspNetUser.Email);
                }
                else
                {

                }
                return Json(new { resp = true });

            }
            var gvid = model.TKBs.Where(g => g.ID_GV == tkb.ID_GV && g.tietHoc.tietBD == tt.tietHoc.tietBD && g.tuanHoc.thuS == tt.tuanHoc.thuS && g.ID_hocKy == tt.ID_hocKy).ToList().Count(g => g.ID_GV != null);
            if (gvid == 0)
            {
                tt.ID_GV = tkb.ID_GV;
                model.SaveChanges();
                var giangvien = model.nguoiDungs.FirstOrDefault(x => x.ID_dsGV == tt.ID_GV);
                if (giangvien != null)
                {
                    SendEmailToUser(giangvien.AspNetUser.Email);
                }
                else
                {

                }
                return Json(new { result = true });

            }
            else if (gvid == 1)
            {
                return Json(new { result = false });
            }



            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TKBDetails(int? id)
        {
            model.Configuration.ProxyCreationEnabled = false;
            var tkbdt = model.TKBs.FirstOrDefault(x => x.ID == id);
            var mdt = model.monHocs.FirstOrDefault(x => x.ID == tkbdt.ID_monHoc);
            var lhpdt = model.hocPhans.FirstOrDefault(x => x.ID == tkbdt.ID_hocPhan);
            var phdt = model.phongHocs.FirstOrDefault(x => x.ID == tkbdt.ID_Phong);
            var tuandt = model.tuanHocs.FirstOrDefault(x => x.ID == tkbdt.ID_Tuan);
            var tdt = model.tietHocs.FirstOrDefault(x => x.ID == tkbdt.ID_Tiet);
            //string mm = tkbdt.monHoc.maMon;
            //string mlhp = tkbdt.hocPhan.maLHP;
            //string tmh = tkbdt.monHoc.tenMon;
            //string lhp = tkbdt.hocPhan.loaiHP;
            //string ph = tkbdt.phongHoc.maPhong;
            //string tuanday = tkbdt.tuanHoc.tuanHoc1;

            var mm = mdt.maMon;
            var mlhp = lhpdt.maLHP;
            var tmh = mdt.tenMon;
            var lhp = lhpdt.loaiHP;
            var ph = phdt.maPhong;
            var tuanday = tuandt.tuanHoc1;
            var tiethoc = tdt.tietHoc1;
            var abcdef = new { a = mm, b = mlhp, c = tmh, d = lhp, e = ph, f = tuanday, g = tiethoc };
            return Json(abcdef, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LockTKB(int? id)
        {
            model.Database.ExecuteSqlCommand("UPDATE [hocKy] SET lockstat = 1 WHERE ID = @ID_hocKy ", new SqlParameter("@ID_hocKy", id));
            return RedirectToAction("Index1", "Home");
        }

        public ActionResult UnLockTKB(int? id)
        {
            model.Database.ExecuteSqlCommand("UPDATE [hocKy] SET lockstat = 0 WHERE ID = @ID_hocKy ", new SqlParameter("@ID_hocKy", id));
            return RedirectToAction("Index1", "Home");
        }

        public ActionResult CancelAll(int? id)
        {
            var tkb = model.TKBs.OrderBy(x => x.ID).ToList();
            model.Database.ExecuteSqlCommand("UPDATE [TKB] SET ID_GV = null WHERE ID_hocKy = @ID_hocKy ", new SqlParameter("@ID_hocKy", id));
            TempData["cancel"] = 1;
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        [LoginVerification]
        public ActionResult DetailStatisticAll(int? id)
        {
            var gv = model.danhsachGVs.OrderBy(x => x.ID).ToList();
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            ViewBag.thk = tkb1.tenHK;
            TempData["idhk"] = tkb1.ID;
            ViewBag.kocogv = null;
            var gv1 = model.TKBs.OrderBy(x => x.ID);
            var kogv = model.TKBs.OrderBy(x => x.ID_hocKy == id);
            foreach (var item in kogv.Select(x => x.ID_GV))
            {
                if (item.ToString() == null)
                {
                    ViewBag.kocogv = 1;
                }
                else
                {
                    ViewBag.kocogv = 2;
                }
            }
            

            //ViewBag.tengv = Session["hoten"];
            //ViewBag.maid = Session["id1"];
            //ViewBag.active = 7;
            //ViewBag.year = year;
            //int result = Int32.Parse(year);
            //ViewBag.sumclassweek = model.TKBs.Where(x => x.ID_GV == cl.ID && x.ID_hocKy == idhk).ToList().Count();
            //ViewBag.sumgghk = (decimal)(ViewBag.sumclassweek * 150) / 60;
            //ViewBag.sumsubhk = model.ACCOUNTs.Where(x => x.DATE_OF_REGISTRATION.Value.Year == result).ToList().Count();
            return View();

        }



        public void SendEmailToUser(string emailId)
        {
            var GenarateUserVerificationLink = ":18080/SEP24Team13/Admin/Auth/UserVerification/";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("cuong.187pm06554@vanlanguni.vn", "TEAM14"); // set your email    
            var fromEmailpassword = "yolooo123"; // Set your password     
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);


            Message.Subject = " Bạn đã có lịch dạy ";
            Message.Body = "<br/> Xin chào." +
                           "<br/> Bạn đã có lịch dạy học tại website Quản Lý và Phân Công của Team 14." +
                           
                           "<br/> Vui lòng truy cập vào website để xem thời khóa biểu của bạn"+
                            "<br/> https://www.youtube.com/watch?v=AaF7rXatU9E";
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }

    }
}