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
    
    public partial class hocKy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hocKy()
        {
            this.TKBs = new HashSet<TKB>();
        }
    
        public int ID { get; set; }
        public string tenHK { get; set; }
        public string namBD { get; set; }
        public string namKT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKB> TKBs { get; set; }
    }
}