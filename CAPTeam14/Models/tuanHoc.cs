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
    
    public partial class tuanHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tuanHoc()
        {
            this.TKBs = new HashSet<TKB>();
        }
    
        public int ID { get; set; }
        public Nullable<int> thuS { get; set; }
        public Nullable<int> tuanBD { get; set; }
        public string tuanHoc1 { get; set; }
        public Nullable<int> tuanKT { get; set; }
        public string thu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKB> TKBs { get; set; }
    }
}
