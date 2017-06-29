using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Phyah.Configuration;
using Phyah.Huaxue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Biz
{
    public class DataContext : DbContext
    {
        public DbSet<Users> Users { set; get; }
        
        public DbSet<Pages> Pages { set; get; }
        public DbSet<Card> Card { set; get; }
        public DbSet<Modules> Modules { set; get; }
        public DbSet<PageModules> PageModules { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(AppSetting.AppSettings["HuaxueConnectionString"].ToString());
    }
}
