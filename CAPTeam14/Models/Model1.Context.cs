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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CP24Team14Entities : DbContext
    {
        public CP24Team14Entities()
            : base("name=CP24Team14Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<hocKy> hocKies { get; set; }
        public virtual DbSet<hocPhan> hocPhans { get; set; }
        public virtual DbSet<lopHoc> lopHocs { get; set; }
        public virtual DbSet<monHoc> monHocs { get; set; }
        public virtual DbSet<Nganh> Nganhs { get; set; }
        public virtual DbSet<nguoiDung> nguoiDungs { get; set; }
        public virtual DbSet<phongHoc> phongHocs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tietHoc> tietHocs { get; set; }
        public virtual DbSet<TKB> TKBs { get; set; }
        public virtual DbSet<tuanHoc> tuanHocs { get; set; }
    }
}
