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

    public partial class hocKy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hocKy()
        {
            this.TKBs = new HashSet<TKB>();
        }
    
        public int ID { get; set; }
        [RegularExpression(@"^\S+(?:\s\S+)*$", ErrorMessage = "Tên học kì không chứa khoảng trắng")]
        public string tenHK { get; set; }
        [RegularExpression(@"^\S+(?:\s\S+)*$", ErrorMessage = "Năm bắt đầu không chứa khoảng trắng")]
        public string namBD { get; set; }
        [RegularExpression(@"^\S+(?:\s\S+)*$", ErrorMessage = "Năm kết thúc không chứa khoảng trắng")]
        public string namKT { get; set; }
        public Nullable<int> ID_nganh { get; set; }
        public Nullable<int> ID_lop { get; set; }
        public Nullable<int> ID_tuan { get; set; }
        public Nullable<bool> lockstat { get; set; }
        public Nullable<int> ID_gv { get; set; }
    
        public virtual danhsachGV danhsachGV { get; set; }
        public virtual lopHoc lopHoc { get; set; }
        public virtual Nganh Nganh { get; set; }
        public virtual tuan tuan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKB> TKBs { get; set; }
    }
}
