using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace UplanServer
{
    public class QoEDbContext:DbContext
    {
        //web api resutful 
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
        public DbSet<QoEMissionInfo> QoEMissionTable { get; set; }
        public DbSet<SysCacheInfo> SysCacheTable { get; set; }
        public DbSet<QoEBlackPoint> QoEBlackPointTable { get; set; }
        public DbSet<SysAutoBusInfo> SysAutoBusTable { get; set; }
        public DbSet<SysTJCellInfo> SysTJCellTable { get; set; }
        public DbSet<AppExceptionInfo> AppExceptionTable { get; set; }

        public int Update<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties) where TEntity : class
        {
            var dbEntityEntry = this.Entry(entity);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {

                foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
                {
                    string pName = property;
                    var original = dbEntityEntry.OriginalValues.GetValue<object>(pName);
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(pName);
                    if (original != null && !original.Equals(current))
                    {
                        dbEntityEntry.Property(pName).IsModified = true;
                    }
                }
            }
            return this.SaveChanges();
        }
    }
}