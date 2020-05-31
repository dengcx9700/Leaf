using Leaf.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Leaf.Data
{
    public class LeafDbContext : IdentityDbContext<LUser, LRole, long>
    {
        public LeafDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public LeafDbContext()
        {

        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<OpenApp> OpenApps { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Article>().HasIndex(nameof(Article.DocumentId));
            builder.Entity<OpenApp>().HasIndex(nameof(OpenApp.AppKey));
            builder.Entity<LUser>().ToTable("users");
            builder.Entity<LRole>().ToTable("roles");
            builder.Entity<IdentityUserClaim<long>>().ToTable("userclaims");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("roleclaims");
            builder.Entity<IdentityUserRole<long>>().ToTable("userroles");
            builder.Entity<IdentityUserLogin<long>>().ToTable("userlogins");
            builder.Entity<IdentityUserToken<long>>().ToTable("usertokens");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=leafdb; User=root;Password=355343;");
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
