using System;
using System.Collections.Generic;
using System.Text;
using Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser,MyRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Claims> Claims { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claims>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("Claims_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Key).IsRequired();

                entity.Property(e => e.Value).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
