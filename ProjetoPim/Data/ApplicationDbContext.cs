using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoPim.Models;

namespace ProjetoPim.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
   //     public object RoleManager { get; internal set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ApplicationUser>().ToTable("Usuario");

           // builder.Entity<ApplicationUser>(b =>
          //  {
          //      b.Property(au => au.Email)
           //     .HasColumnName("NomeUsuario");
           // });
            //builder.Entity<ApplicationRole>()

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<Chamado> Chamado { get; set; }
        public DbSet<IdentityRole> identityRole { get; set; }

    }
}
