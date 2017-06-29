

namespace Portal.Entity
{
    using Microsoft.EntityFrameworkCore;
    using Phyah.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using JetBrains.Annotations;
    using MySQL.Data.EntityFrameworkCore.Extensions;

    public class DataContext : DbContext
    {

        public DbSet<Card> Card { set; get; }

        public DbSet<Article> Article { set; get; }
        public DbSet<Contactor> Contactor { set; get; }
        public DbSet<PartialView> PartialView { set; get; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Rel_Menus> Rel_Menus { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(AppSetting.AppSettings["HuaxueConnectionString"].ToString());
    }

}