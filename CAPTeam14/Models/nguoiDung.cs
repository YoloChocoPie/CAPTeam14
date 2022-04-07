﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAPTeam14.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class nguoiDung
    {
        public int ID { get; set; }
        public string userID { get; set; }
        [RegularExpression(@"^\S+(?:\s\S+)*$", ErrorMessage = "Dữ liệu không có khoảng trắng")]
        public string maGV { get; set; }
        [RegularExpression(@"^\S+(?:\s\S+)*$", ErrorMessage = "Dữ liệu không có khoảng trắng")]
        public string tenGV { get; set; }
        public Nullable<bool> loaiGV { get; set; }
        public string khoa { get; set; }
        public Nullable<bool> gioiTinh { get; set; }
        public Nullable<int> role { get; set; }
        [RegularExpression(@"^\S+(?:\s\S+)*$", ErrorMessage = "Dữ liệu không có khoảng trắng")]
        public Nullable<int> sdt { get; set; }
        public Nullable<int> ID_dsGV { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual danhsachGV danhsachGV { get; set; }
    }
}
