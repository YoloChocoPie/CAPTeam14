//------------------------------------------------------------------------------
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
    
    public partial class danhsachGV
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public danhsachGV()
        {
            this.nguoiDungs = new HashSet<nguoiDung>();
        }
    
        public int ID { get; set; }
        public string maGV { get; set; }
        public string tenGV { get; set; }
        public string loaiGV { get; set; }
        public string khoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nguoiDung> nguoiDungs { get; set; }
    }
}