using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tb.datalayer.Models;


namespace tb.datalayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
     : base(options)
        { }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var valueComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet().ToList());

            modelBuilder.Entity<Card>().HasIndex(c => c.Id);
            modelBuilder.Entity<Card>().HasOne(c => c.Language).WithMany(l => l.Cards).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
            .Property(e => e.Tabus)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v))
            .Metadata.SetValueComparer(valueComparer);

            modelBuilder.Entity<Language>().HasData(new Language { Id = 1, LanguageCode = "de", Name = "Deutsch" });
            modelBuilder.Entity<Card>().HasData(new Card { Id = 1, Description = "Test", LanguageId = 1, Tabus = new List<string> { "1", "2" } });
        }
    }
}
