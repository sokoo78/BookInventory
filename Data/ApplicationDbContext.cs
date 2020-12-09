using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookInventory.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookInventory.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>()
                        .HasMany(a => a.Books)
                        .WithOne(b => b.Author)
                        .HasForeignKey(b => b.AuthorId);
        }
    }
}
