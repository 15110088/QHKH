﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QHKH.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KHQHEntities : DbContext
    {
        public KHQHEntities()
            : base("name=KHQHEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<CONGTRINHDUAN> CONGTRINHDUANs { get; set; }
        public virtual DbSet<DM_CHUYENMUCDICH> DM_CHUYENMUCDICH { get; set; }
        public virtual DbSet<DM_KVHC> DM_KVHC { get; set; }
        public virtual DbSet<DM_LOAICONGTRINH> DM_LOAICONGTRINH { get; set; }
        public virtual DbSet<DM_LOAIKHUCHUCNANG> DM_LOAIKHUCHUCNANG { get; set; }
        public virtual DbSet<DM_MUCDICHSUDUNG> DM_MUCDICHSUDUNG { get; set; }
        public virtual DbSet<HIENTRANG> HIENTRANGs { get; set; }
        public virtual DbSet<KEHOACH> KEHOACHes { get; set; }
        public virtual DbSet<KEHOACH_CMD> KEHOACH_CMD { get; set; }
        public virtual DbSet<KEHOACH_CSD> KEHOACH_CSD { get; set; }
        public virtual DbSet<KEHOACH_THUHOI> KEHOACH_THUHOI { get; set; }
        public virtual DbSet<KHUCHUCNANG> KHUCHUCNANGs { get; set; }
        public virtual DbSet<KHUCHUCNANG_MDSD> KHUCHUCNANG_MDSD { get; set; }
        public virtual DbSet<KYQUYHOACHKEHOACH> KYQUYHOACHKEHOACHes { get; set; }
        public virtual DbSet<MAP_CONFIG> MAP_CONFIG { get; set; }
        public virtual DbSet<PHANBO_XACDINH> PHANBO_XACDINH { get; set; }
        public virtual DbSet<QUYHOACH> QUYHOACHes { get; set; }
        public virtual DbSet<TH_CHUCHUYENDATDAI> TH_CHUCHUYENDATDAI { get; set; }
        public virtual DbSet<TH_KYTRUOC> TH_KYTRUOC { get; set; }
        public virtual DbSet<USERTABLE> USERTABLEs { get; set; }
        public virtual DbSet<USERTABLE_LOG> USERTABLE_LOG { get; set; }
        public virtual DbSet<db_error_list> db_error_list { get; set; }
        public virtual DbSet<db_LOB_output> db_LOB_output { get; set; }
        public virtual DbSet<db_storage> db_storage { get; set; }
        public virtual DbSet<t_dbms_sql_bind_variable> t_dbms_sql_bind_variable { get; set; }
        public virtual DbSet<t_dbms_sql_cursor> t_dbms_sql_cursor { get; set; }
        public virtual DbSet<t_dbms_sql_define_column> t_dbms_sql_define_column { get; set; }
        public virtual DbSet<t_dbms_sql_recordset> t_dbms_sql_recordset { get; set; }
    }
}