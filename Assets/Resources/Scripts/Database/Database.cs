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
            modelBuilder.Entity<Domain.DomainProperty>(entity =>
            {
                entity.ToTable("Domain");
                entity.HasKey("id");
                entity.Property("name");
                entity.Property("character");
            });
            modelBuilder.Entity<Tile.TileProperty>(entity =>
            {
                entity.ToTable("Tile");
                entity.HasKey(new string[] { "x", "y" });
                entity.Property("terrain").IsRequired();
                entity.HasOne<Domain.DomainProperty>().WithOne().HasForeignKey(typeof(Tile.TileProperty), "domain");
            });
        }
        public DbSet<Tile.TileProperty> Tile { get; set; }
        public DbSet<Domain.DomainProperty> Domain { get; set; }

    }

    private static void DatabaseCreationCommand()
    {
        DbConnection dbConnection = new NpgsqlConnection("Server=localhost;Port=5432;User Id=Chevauchee; Password=Chevauchee;Database=postgres; pooling=false");
        dbConnection.Open();
        IDbCommand createDB = dbConnection.CreateCommand();
        createDB.CommandText = "CREATE DATABASE \"Chevauchee\";";
        createDB.ExecuteNonQuery();
        dbConnection.ChangeDatabase("Chevauchee");
        IDbCommand addExtensions = dbConnection.CreateCommand();
        addExtensions.CommandText = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";";
        addExtensions.ExecuteNonQuery();
        IDbCommand createSchema = dbConnection.CreateCommand();
        createSchema.CommandText = "CREATE SCHEMA Chevauchee; ";
        createSchema.ExecuteNonQuery();
        using (DatabaseManager db = new DatabaseManager())
        {
            if(!db.Database.EnsureCreated())
            {
                throw new NonExistantKeyError("Database isn't found/created");
            }
            db.SaveChanges();
        }
        dbConnection.Close();
        /*IDbCommand createTable = dbConnection.CreateCommand();
        createTable.CommandText = "CREATE TABLE chevauchee.character(id uuid PRIMARY KEY, name VARCHAR(20), gold integer, symbol_id uuid);";
        createTable.ExecuteNonQuery();
        createTable.CommandText = "CREATE TABLE chevauchee.kingdom(id uuid PRIMARY KEY, name varchar(20));";
        createTable.ExecuteNonQuery();
        createTable.CommandText = "CREATE TABLE chevauchee.nobility(character_id uuid, kingdom_id uuid, rank integer);";
        createTable.ExecuteNonQuery();
        createTable.CommandText = "CREATE TABLE chevauchee.domain(id uuid PRIMARY KEY, character_id uuid, name varchar(20))";
        createTable.ExecuteNonQuery();
        createTable.CommandText = "Create TABLE chevauchee.Tile(x integer NOT NULL, y integer NOT NULL, domain_id uuid, terrain integer, elevation integer, precipitation integer, CONSTRAINT position PRIMARY KEY(X, Y));";
        createTable.ExecuteNonQuery();
        createTable.CommandText = "CREATE TABLE chevauchee.building(id uuid PRIMARY KEY, tile_X integer, tile_y integer, type integer, maintenance integer);";
        createTable.ExecuteNonQuery();
        IDbCommand defaultValues = dbConnection.CreateCommand();
        defaultValues.CommandText = "INSERT INTO chevauchee.domain (id) VALUES ('" + Guid.Empty + "');";
        defaultValues.ExecuteNonQuery();
        IDbCommand foreignKey = dbConnection.CreateCommand();
        foreignKey.CommandText = "ALTER TABLE chevauchee.domain ADD FOREIGN KEY(character_id) REFERENCES chevauchee.character(id); ";
        foreignKey.ExecuteNonQuery();
        foreignKey.CommandText = "ALTER TABLE chevauchee.nobility ADD FOREIGN KEY(character_id) REFERENCES chevauchee.character(id), ADD FOREIGN KEY(kingdom_id) REFERENCES chevauchee.kingdom(id); ";
        foreignKey.ExecuteNonQuery();
        foreignKey.CommandText = "ALTER TABLE chevauchee.tile ADD FOREIGN KEY(domain_id) REFERENCES chevauchee.domain(id);";
        foreignKey.ExecuteNonQuery();
        foreignKey.CommandText = "ALTER TABLE chevauchee.building ADD FOREIGN KEY(tile_x, tile_y) REFERENCES chevauchee.tile(x,y);";
        foreignKey.ExecuteNonQuery();*/
    }

    public static void KillDatabase()
    {
        DbConnection dbConnection = new NpgsqlConnection("Server=localhost;Port=5432;User Id=Chevauchee; Password=Chevauchee;Database=Chevauchee; pooling=false");
        IDbCommand deleteDatabase = dbConnection.CreateCommand();
        dbConnection.ChangeDatabase("postgres");
        deleteDatabase.CommandText = "DROP DATABASE chevauchee;";
        deleteDatabase.ExecuteNonQuery();
        dbConnection.Close();
    }
    // Use this for initialization
    public static void StartDatabase()
    {
        DatabaseCreationCommand();
    }
}
