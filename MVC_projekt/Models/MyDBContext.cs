using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_projekt.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("Database") { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorGroup> AuthorGroups { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookItem> BookItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Fee>Fees { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<LabelGroup> LabelGroups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SearchResult> SearchResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookItem>()
                .HasOptional(s => s.Category)
                .WithRequired(ad => ad.BookItem);

        }

    }
}