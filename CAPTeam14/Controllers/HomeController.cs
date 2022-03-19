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


// 02/03/2022 cập nhập lại luồng đi Import, cải thiện tốc độ performance bằng cách chuyển hết định dạng sang chuỗi
// 02/03/22 04:42 đang dừng lại ở câu lệnh kiểm tra học phần đã tồn tại hay chưa. Đã cập nhập database và thêm table Học Kỳ
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
            var tkb = model.hocKies.OrderByDescending(x => x.ID).ToList();
            var test = model.TKBs.OrderByDescending(x => x.ID).Count();
            ViewBag.test = test;
           

            return View(tkb);
        }

        public ActionResult Index(int? id)
        {
            // lấy danh sách thời khóa biểu
            var tkb = model.TKBs.OrderByDescending(x => x.ID).ToList();
            // lấy danh sách học kì
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            //hiển thị tên học kì đã chọn
            ViewBag.test2 = tkb1.tenHK;
            // lấy thông tin ID học kì đã chọn trong thời khóa biểu
            ViewBag.test = id;
           
            ViewBag.gv = model.nguoiDungs.Where(x=> x.role == 4).OrderByDescending(x => x.ID).ToList();
            return View(tkb);
        }
       // Query Delete TKB
        public ActionResult DeleteTKB(int? id)
        {
            ViewBag.test = model.TKBs.Where(x => x.ID_hocKy == id).OrderByDescending(x => x.ID).ToList();

            model.Database.ExecuteSqlCommand("DELETE FROM [TKB] WHERE ID_hocKy = @ID_hocKy ", new SqlParameter("@ID_hocKy", id));
            TempData["ThongBao"] = 1;
            return RedirectToAction("Index1", "Home");
        }
        [HttpGet]
        // Luồng đi mới của Import
        public ActionResult Catalog(int? id)
        {

            ViewBag.hocky = model.hocKies.OrderByDescending(x => x.ID).ToList();
            // lấy id của học kì đã chọn
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            // hiển thị tên học kì đã chọn
            ViewBag.test2 = tkb1.tenHK;
            ViewBag.test1 = id;
            return View();
        }
      
        public ActionResult Catalog(TKB tkb, int? id)
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
                     var hocphan = new hocPhan(); // 1
                    var lop = new lopHoc(); // 2
                    var mon = new monHoc(); // 3 
                    var nganh = new Nganh(); // 4
                    var phong = new phongHoc(); // 5
                    var tiet = new tietHoc(); // 6
                    var tuan = new tuanHoc(); // 7
                    var tkbTong = new TKB(); // 8





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
                    String maGV = cot[15].ToString(); // không dùng
                    String tenGV = cot[16].ToString(); // không dùng
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

                    // 30 - 3 = 27 

                    // table hocPhans
                    var checkmagocLHP = model.hocPhans.FirstOrDefault(x => x.maGocLHP == magocLHP); // 1
                    var checkmaLHP = model.hocPhans.FirstOrDefault(x => x.maLHP == maLHP); // 2
                    var checkloaiHP = model.hocPhans.FirstOrDefault(x => x.loaiHP == loaiHP); // 3
                    var checktinhtrangLHP = model.hocPhans.FirstOrDefault(x => x.tinhtrangLHP == tinhtrangLHP); // 4
                    var checkTSMH = model.hocPhans.FirstOrDefault(x => x.TSMH == TSMH); // 5

                    // table lopHocs
                    var checklopHoc = model.lopHocs.FirstOrDefault(x => x.maLop == maLop); // 6

                    // table tuanHoc
                    var checkthuS = model.tuanHocs.FirstOrDefault(x => x.thuS == thuS); // 7
                    var checktuanBD = model.tuanHocs.FirstOrDefault(x => x.tuanBD == tuanBD); // 8
                    var checktuanHoc = model.tuanHocs.FirstOrDefault(x => x.tuanHoc1 == tuanHoc); // 9
                    var checktuanKT = model.tuanHocs.FirstOrDefault(x => x.tuanKT == tuanKT); // 10
                    var checkthu = model.tuanHocs.FirstOrDefault(x => x.thu == thu); // 11

                    // table Nganh
                    var checkmaNganh = model.Nganhs.FirstOrDefault(x => x.maNganh == maNganh); // 12
                    var checktenNganh = model.Nganhs.FirstOrDefault(x => x.tenNganh == tenNganh); // 13

                    // table tietHocs
                    var checktongTiet = model.tietHocs.FirstOrDefault(x => x.tongTiet == tongTiet); // 14
                    var checksoTiet = model.tietHocs.FirstOrDefault(x => x.soTiet == soTiet); // 15
                    var checktietHoc = model.tietHocs.FirstOrDefault(x => x.tietHoc1 == tietHoc); // 16
                    var checktietS = model.tietHocs.FirstOrDefault(x => x.tietS == tietS); // 17
                    var checktietBD = model.tietHocs.FirstOrDefault(x => x.tietBD == tietBD); // 18

                    // table monHoc
                    var checkmaMon = model.monHocs.FirstOrDefault(x => x.maMon == maMon); // 19
                    var checktenMon = model.monHocs.FirstOrDefault(x => x.tenMon == tenMon); // 20
                    var checkTC = model.monHocs.FirstOrDefault(x => x.tinChi == soTC); // 21

                    // table phongHoc
                    var checkloaiPhong = model.phongHocs.FirstOrDefault(x => x.loaiPhong == PHX); // 22
                    var checksucChua = model.phongHocs.FirstOrDefault(x => x.sucChua == sucChua); // 23
                    var checksiSo = model.phongHocs.FirstOrDefault(x => x.siSo == siSo); // 24
                    var checktrong = model.phongHocs.FirstOrDefault(x => x.trong == Trong); // 25
                    var checksoSVDK = model.phongHocs.FirstOrDefault(x => x.soSVDK == soSVDK); // 26
                    var checkmaPhong = model.phongHocs.FirstOrDefault(x => x.maPhong == phongHoc); // 27




                    // Học phần

                    // câu lệnh kiểm tra xem học phần đã tồn tại hay chưa
                    // nếu chưa tồn tại thì tạo mới Học Phần
                    if (checkmagocLHP == null && checkmaLHP == null)
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
                    // còn nếu đã tồn tại một trong những dữ liệu trên rồi thì lưu giữ dữ liệu đã tồn tại và tiếp tục vòng lặp để kiểm tra 
                    // xem có dữ liệu nào khác thì
                    else
                    {
                        hocphan = checkmagocLHP;
                        hocphan = checkmaLHP;
                        hocphan = checkloaiHP;
                        hocphan = checktinhtrangLHP;
                        hocphan = checkTSMH;
                        tkbTong.ID_hocPhan = checkmagocLHP.ID;
                    }

                    // lớp học
                    if (checklopHoc == null)
                    {
                        lop = new lopHoc
                        {
                            maLop = maLop
                        };
                        model.lopHocs.Add(lop);
                        model.SaveChanges();

                        tkbTong.ID_Lop = lop.ID;

                    }
                    //
                    else
                    {
                        lop = checklopHoc;
                        tkbTong.ID_Lop = checklopHoc.ID;
                    }

                    // tuần học
                    if (checkthuS == null || checktuanBD == null || checktuanHoc == null || checktuanKT == null || checkthu == null)
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
                    //
                    else
                    {
                        tuan = checkthuS;
                        tuan = checktuanBD;
                        tuan = checktuanHoc;
                        tuan = checktuanKT;
                        tuan = checkthu;

                        tkbTong.ID_Tuan = checkthu.ID;
                    }

                    // ngành
                    if (checkmaNganh == null || checktenNganh == null)
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
                    //
                    else
                    {
                        nganh = checkmaNganh;
                        nganh = checktenNganh;
                        tkbTong.ID_Nganh = checkmaNganh.ID;
                    }

                    // tiết học
                    if (checktongTiet == null || checksoTiet == null || checktietHoc == null || checktietS == null || checktietBD == null)
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
                    //
                    else
                    {
                        tiet = checktongTiet;
                        tiet = checksoTiet;
                        tiet = checktietHoc;
                        tiet = checktietS;
                        tiet = checktietBD;
                        tkbTong.ID_Tiet = checktietBD.ID;
                    }

                    // môn học
                    if (checkmaMon == null || checktenMon == null )
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
                    //
                    else
                    {
                        mon = checktenMon;
                        mon = checkmaMon;
                        mon = checkTC;
                        tkbTong.ID_monHoc = checktenMon.ID;
                    }

                    // phòng học
                    //  checksucChua == null || checksiSo == null || checktrong == null || checksoSVDK == null ||
                    if (checkloaiPhong == null || checkmaPhong == null || checksucChua == null || checksiSo == null || checktrong == null || checksoSVDK == null )
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
                    //
                    else
                    {
                        phong = checkloaiPhong;
                        phong = checksucChua;
                        phong = checksiSo;
                        phong = checktrong;
                        phong = checksoSVDK;
                        phong = checkmaPhong;
                        tkbTong.ID_Phong = checkmaPhong.ID;
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

                    var checktkbhp = model.TKBs.FirstOrDefault(x => x.ID_hocPhan == hocphan.ID);

                    if (checktkbhp == null)
                    {
                        tkbTong.ID_hocKy = id;
                        model.TKBs.Add(tkbTong);
                        model.SaveChanges();
                    }


                }
            }
            // kết thúc vòng lặp và ngưng đọc dữ liệu sau 29 cột
            IEDreader.Close();
            TempData["ThongBao"] = 1;
            return RedirectToAction("Catalog", "Home");
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


        [HttpGet]
        // Thay thế thời khóa biểu
        public ActionResult Catalog1(int? id)
        {

            ViewBag.hocky = model.hocKies.OrderByDescending(x => x.ID).ToList();
            // lấy id của học kì đã chọn
            var tkb1 = model.hocKies.FirstOrDefault(x => x.ID == id);
            // hiển thị tên học kì đã chọn
            ViewBag.test2 = tkb1.tenHK;
            ViewBag.test1 = id;
            return View();
        }

        public ActionResult Catalog1(TKB tkb, int? id)
        {

            model.Database.ExecuteSqlCommand("DELETE FROM [TKB] WHERE ID_hocKy = @ID_hocKy ", new SqlParameter("@ID_hocKy", id));
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
                    String maGV = cot[15].ToString(); // không dùng
                    String tenGV = cot[16].ToString(); // không dùng
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

                    // 30 - 3 = 27 

                    // table hocPhans
                    var checkmagocLHP = model.hocPhans.FirstOrDefault(x => x.maGocLHP == magocLHP); // 1
                    var checkmaLHP = model.hocPhans.FirstOrDefault(x => x.maLHP == maLHP); // 2
                    var checkloaiHP = model.hocPhans.FirstOrDefault(x => x.loaiHP == loaiHP); // 3
                    var checktinhtrangLHP = model.hocPhans.FirstOrDefault(x => x.tinhtrangLHP == tinhtrangLHP); // 4
                    var checkTSMH = model.hocPhans.FirstOrDefault(x => x.TSMH == TSMH); // 5

                    // table lopHocs
                    var checklopHoc = model.lopHocs.FirstOrDefault(x => x.maLop == maLop); // 6

                    // table tuanHoc
                    var checkthuS = model.tuanHocs.FirstOrDefault(x => x.thuS == thuS); // 7
                    var checktuanBD = model.tuanHocs.FirstOrDefault(x => x.tuanBD == tuanBD); // 8
                    var checktuanHoc = model.tuanHocs.FirstOrDefault(x => x.tuanHoc1 == tuanHoc); // 9
                    var checktuanKT = model.tuanHocs.FirstOrDefault(x => x.tuanKT == tuanKT); // 10
                    var checkthu = model.tuanHocs.FirstOrDefault(x => x.thu == thu); // 11

                    // table Nganh
                    var checkmaNganh = model.Nganhs.FirstOrDefault(x => x.maNganh == maNganh); // 12
                    var checktenNganh = model.Nganhs.FirstOrDefault(x => x.tenNganh == tenNganh); // 13

                    // table tietHocs
                    var checktongTiet = model.tietHocs.FirstOrDefault(x => x.tongTiet == tongTiet); // 14
                    var checksoTiet = model.tietHocs.FirstOrDefault(x => x.soTiet == soTiet); // 15
                    var checktietHoc = model.tietHocs.FirstOrDefault(x => x.tietHoc1 == tietHoc); // 16
                    var checktietS = model.tietHocs.FirstOrDefault(x => x.tietS == tietS); // 17
                    var checktietBD = model.tietHocs.FirstOrDefault(x => x.tietBD == tietBD); // 18

                    // table monHoc
                    var checkmaMon = model.monHocs.FirstOrDefault(x => x.maMon == maMon); // 19
                    var checktenMon = model.monHocs.FirstOrDefault(x => x.tenMon == tenMon); // 20
                    var checkTC = model.monHocs.FirstOrDefault(x => x.tinChi == soTC); // 21

                    // table phongHoc
                    var checkloaiPhong = model.phongHocs.FirstOrDefault(x => x.loaiPhong == PHX); // 22
                    var checksucChua = model.phongHocs.FirstOrDefault(x => x.sucChua == sucChua); // 23
                    var checksiSo = model.phongHocs.FirstOrDefault(x => x.siSo == siSo); // 24
                    var checktrong = model.phongHocs.FirstOrDefault(x => x.trong == Trong); // 25
                    var checksoSVDK = model.phongHocs.FirstOrDefault(x => x.soSVDK == soSVDK); // 26
                    var checkmaPhong = model.phongHocs.FirstOrDefault(x => x.maPhong == phongHoc); // 27




                    // Học phần

                    // câu lệnh kiểm tra xem học phần đã tồn tại hay chưa
                    // nếu chưa tồn tại thì tạo mới Học Phần
                    if (checkmagocLHP == null && checkmaLHP == null )
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
                    // còn nếu đã tồn tại một trong những dữ liệu trên rồi thì lưu giữ dữ liệu đã tồn tại và tiếp tục vòng lặp để kiểm tra 
                    // xem có dữ liệu nào khác thì
                    else
                    {
                        hocphan = checkmagocLHP;
                        hocphan = checkmaLHP;
                        hocphan = checkloaiHP;
                        hocphan = checktinhtrangLHP;
                        hocphan = checkTSMH;
                        tkbTong.ID_hocPhan = hocphan.ID;
                    }

                    // lớp học
                    if (checklopHoc == null)
                    {
                        lop = new lopHoc
                        {
                            maLop = maLop
                        };
                        model.lopHocs.Add(lop);
                        model.SaveChanges();

                        tkbTong.ID_Lop = lop.ID;

                    }
                    //
                    else
                    {
                        lop = checklopHoc;
                        tkbTong.ID_Lop = lop.ID;
                    }

                    // tuần học
                    if (checkthuS == null && checktuanBD == null && checktuanHoc == null && checktuanKT == null && checkthu == null)
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
                    //
                    else
                    {
                        tuan = checkthuS;
                        tuan = checktuanBD;
                        tuan = checktuanHoc;
                        tuan = checktuanKT;
                        tuan = checkthu;

                        tkbTong.ID_Tuan = tuan.ID;
                    }

                    // ngành
                    if (checkmaNganh == null && checktenNganh == null)
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
                    //
                    else
                    {
                        nganh = checkmaNganh;
                        nganh = checktenNganh;
                        tkbTong.ID_Nganh = nganh.ID;
                    }

                    // tiết học
                    if (checktongTiet == null && checksoTiet == null && checktietHoc == null && checktietS == null && checktietBD == null)
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
                    //
                    else
                    {
                        tiet = checktongTiet;
                        tiet = checksoTiet;
                        tiet = checktietHoc;
                        tiet = checktietS;
                        tiet = checktietBD;
                        tkbTong.ID_Tiet = tiet.ID;
                    }

                    // môn học
                    if (checkmaMon == null && checktenMon == null )
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
                    //
                    else
                    {
                        mon = checkmaMon;
                        mon = checktenMon;
                        mon = checkTC;
                        tkbTong.ID_monHoc = mon.ID;
                    }

                    // phòng học
                    //  checksucChua == null || checksiSo == null || checktrong == null || checksoSVDK == null ||
                    if (checkloaiPhong == null && checkmaPhong == null)
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
                    //
                    else
                    {
                        phong = checkloaiPhong;
                        phong = checksucChua;
                        phong = checksiSo;
                        phong = checktrong;
                        phong = checksoSVDK;
                        phong = checkmaPhong;
                        tkbTong.ID_Phong = phong.ID;
                    }
                    // kiểm tra dữ liệu thời khóa biểu đã tồn tại hay chưa
                    // table tkb
                    var checktkbhp = model.TKBs.FirstOrDefault(x => x.ID_hocPhan == hocphan.ID);
                    var checktkblop = model.TKBs.FirstOrDefault(x => x.ID_Lop == lop.ID);
                    var checktkbtuan = model.TKBs.FirstOrDefault(x => x.ID_Tuan == tuan.ID);
                    var checktkbnganh = model.TKBs.FirstOrDefault(x => x.ID_Nganh == nganh.ID);
                    var checktkbtiet = model.TKBs.FirstOrDefault(x => x.ID_Tiet == tiet.ID);
                    var checktkbmon = model.TKBs.FirstOrDefault(x => x.ID_monHoc == mon.ID);
                    var checktkbphong = model.TKBs.FirstOrDefault(x => x.ID_Phong == phong.ID);


                    if (checktkbhp == null || checktkblop == null || checktkbtuan == null || checktkbnganh == null ||
                        checktkbtiet == null || checktkbmon == null || checktkbphong == null)
                    {
                        tkbTong.ID_hocKy = id;
                        model.TKBs.Add(tkbTong);
                        model.SaveChanges();
                    }


                }
            }
            // kết thúc vòng lặp và ngưng đọc dữ liệu sau 29 cột
            IEDreader.Close();
            TempData["ThongBao"] = 1;
            return RedirectToAction("Catalog1", "Home");
        }



        [HttpPost]
        public ActionResult PhanCong(int? id,TKB tkb, int? idGV)
        {
            
            var tt = model.TKBs.FirstOrDefault(x => x.ID == id);
            tt.ID_GV = tkb.ID_GV;
            model.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

      
       
      


    }
}