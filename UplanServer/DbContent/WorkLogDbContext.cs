using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class WorkLogDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //一定要重写这个方法，用户名必须在这里键入，并且必须大写！！！！！
            modelBuilder.HasDefaultSchema("WORK".ToUpper());
            //禁止EF框架在表名后面加s
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<ProjectFileInfo> ProjectFileTable { get; set; }

    }
}