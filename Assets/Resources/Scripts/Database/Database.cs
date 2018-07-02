using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using Npgsql;
using System.Linq;


using UnityEngine;
public partial class Database
{
    public class DatabaseManager : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Database=Chevauchee;Username=Chevauchee;Password=Chevauchee");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Chevauchee");
            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("Character");
                entity.HasKey("id");
                entity.Property("name");
            });
            modelBuilder.Entity<Kingdom.Nobility>(entity =>
            {
                entity.ToTable("Nobility");
                entity.HasOne<Kingdom>().WithMany().HasForeignKey("kingdom");
                entity.HasOne<Character>().WithMany().HasForeignKey("character");
                entity.HasKey(new string[] { "kingdom", "character" });
                entity.Property("title").IsRequired();
            });
            modelBuilder.Entity<Kingdom>(entity =>
            {
                entity.ToTable("Kingdom");
                entity.HasKey("id");
                entity.Property("name");
            });
            modelBuilder.Entity<Domain>(entity =>
            {
                entity.ToTable("Domain");
                entity.HasKey("id");
                entity.Property("name");
                entity.HasOne<Character>().WithMany().HasForeignKey("character");
            });
            modelBuilder.Entity<Tile>(entity =>
            {
                entity.ToTable("Tile");
                entity.HasKey(new string[] { "x", "y" });
                entity.Property("terrain").IsRequired();
                entity.HasOne<Domain>().WithMany().HasForeignKey("domain");
            });
        }
        public DbSet<Character> Character { get; set; }
        public DbSet<Kingdom.Nobility> Nobility { get; set; }
        public DbSet<Kingdom> Kingdom { get; set; }
        public DbSet<Domain> Domain { get; set; }
        public DbSet<Tile> Tile { get; set; }

    }

    public static void KillDatabase()
    {
        /*DbConnection dbConnection = new NpgsqlConnection("Server=localhost;Port=5432;User Id=Chevauchee; Password=Chevauchee;Database=Chevauchee; pooling=false");
        dbConnection.Open();
        IDbCommand deleteDatabase = dbConnection.CreateCommand();
        dbConnection.ChangeDatabase("postgres");
        deleteDatabase.CommandText = "DROP DATABASE \"Chevauchee\";";
        deleteDatabase.ExecuteNonQuery();
        dbConnection.Close();*/
        NpgsqlConnection.ClearAllPools();
    }
}
