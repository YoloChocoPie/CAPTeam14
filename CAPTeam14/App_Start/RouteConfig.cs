using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CAPTeam14
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //Trang chủ
            routes.MapRoute(
                name: "userdetail",
                url: "HomePage/{userid}",
                defaults: new {controller = "Home",action = "Index1", userid = UrlParameter.Optional}, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
               name: "downloadex",
               url: "download/{userid}",
               defaults: new { controller = "Home", action = "DownloadExcel", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
               name: "downloaddsGV",
               url: "downloaddsGV/{userid}",
               defaults: new { controller = "dsGV", action = "DownloadExcel", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });
            /*routes.MapRoute(
               name: "tkb",
               url: "Thoikhoabieu/{userid}",
               defaults: new { controller = "Home", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });*/

            routes.MapRoute(
              name: "thongketong",
              url: "Thongketong/{userid}",
              defaults: new { controller = "Home", action = "DetailStatisticAll", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            /*routes.MapRoute(
              name: "importtkb",
              url: "Importtkb/{userid}",
              defaults: new { controller = "Home", action = "Catalog", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });*/

            routes.MapRoute(
             name: "exporttkb",
             url: "Exporttkb/{userid}",
             defaults: new { controller = "Home", action = "IndexEx", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            //Phân Quyền
            routes.MapRoute(
            name: "Role",
            url: "Roles/{userid}",
            defaults: new { controller = "giangVien", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "EditRole",
            url: "EditRoles/{userid}",
            defaults: new { controller = "giangVien", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            // Ticket
            routes.MapRoute(
            name: "Ticket",
            url: "Ticket/{userid}",
            defaults: new { controller = "Contact", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Ticketedit",
            url: "TicketEdit/{userid}",
            defaults: new { controller = "Contact", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Ticketcreate",
            url: "TicketCreate/{userid}",
            defaults: new { controller = "Contact", action = "Create", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            // Quản Lý học kì
            routes.MapRoute(
            name: "Hocky",
            url: "Hocky/{userid}",
            defaults: new { controller = "hocKy", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Hockyedit",
            url: "Hockyedit/{userid}",
            defaults: new { controller = "hocKy", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Hockycreate",
            url: "Hockycreate/{userid}",
            defaults: new { controller = "hocKy", action = "Create", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            //Môn học
            routes.MapRoute(
            name: "Monhoc",
            url: "MonHoc/{userid}",
            defaults: new { controller = "monHoc", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Taomonhoc",
            url: "Taomonhoc/{userid}",
            defaults: new { controller = "monHoc", action = "Create", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Editmonhoc",
            url: "Editmonhoc/{userid}",
            defaults: new { controller = "monHoc", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            // Ngành
            routes.MapRoute(
            name: "Nganhhoc",
            url: "Nganhhoc/{userid}",
            defaults: new { controller = "Nganh", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Taonganhhoc",
            url: "Taonganhhoc/{userid}",
            defaults: new { controller = "Nganh", action = "Create", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Editnganhhoc",
            url: "Editnganhhoc/{userid}",
            defaults: new { controller = "Nganh", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            // Lớp học
            routes.MapRoute(
            name: "Lophoc",
            url: "Lophoc/{userid}",
            defaults: new { controller = "lopHoc", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Taolophoc",
            url: "Taolophoc/{userid}",
            defaults: new { controller = "lopHoc", action = "Create", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Editlophoc",
            url: "Editlophoc/{userid}",
            defaults: new { controller = "lopHoc", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            //DsGV
            routes.MapRoute(
            name: "DanhsachGV",
            url: "DanhsachGV/{userid}",
            defaults: new { controller = "dsGV", action = "Index", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            /* routes.MapRoute(
             name: "ImportDanhsachGV",
             url: "ImportDanhsachGV/{userid}",
             defaults: new { controller = "dsGV", action = "Catalog", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });*/

            routes.MapRoute(
            name: "EditdsGV",
            url: "EditdsGV/{userid}",
            defaults: new { controller = "dsGV", action = "Edit", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "thongkeGV",
            url: "thongkeGV/{userid}",
            defaults: new { controller = "giangVien", action = "DetailStatistic", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "thongkecanhan",
            url: "thongkecanhan/{userid}",
            defaults: new { controller = "giangVien", action = "DetailStatistic1", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });
            // Login
            routes.MapRoute(
            name: "Login",
            url: "Dangnhap/{userid}",
            defaults: new { controller = "Account", action = "Login", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            routes.MapRoute(
            name: "Create",
            url: "taotaikhoan/{userid}",
            defaults: new { controller = "Account", action = "Create", userid = UrlParameter.Optional }, namespaces: new[] { string.Format("{0}.Controllers", typeof(RouteConfig).Namespace) });

            //
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index1", id = UrlParameter.Optional }
            );
            

        }

    }

}
