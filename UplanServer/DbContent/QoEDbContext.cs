using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class QoEDbContext:DbContext
    {
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //一定要重写这个方法，用户名必须在这里键入，并且必须大写！！！！！
            modelBuilder.HasDefaultSchema("UPLAN".ToUpper());
            //禁止EF框架在表名后面加s
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<QoEVideoSource> QoEVideoSourceTable { get; set; }
        public DbSet<UserBPInfo> UserBPTable { get; set; }
        public DbSet<DeviceInfo> DeviceTable { get; set; }
        public DbSet<QoEReportIOSInfo> QoERIOSTable { get; set; }
        public DbSet<PhoneInfo> QoERTable { get; set; }
        public DbSet<QoEVideoTable> QoEVideoTable { get; set; }
    }
}