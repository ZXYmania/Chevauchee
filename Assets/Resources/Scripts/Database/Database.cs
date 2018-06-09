using System;
using System.Collections;
using System.Collections.Generic;
//using Mono.Data.Sqlite;
using System.Data;
using Npgsql;
using UnityEngine;

public partial class Database
{
    static IDbConnection dbConnection;
    private static void DatabaseCreationCommand()
    {
        dbConnection = new NpgsqlConnection("Server=localhost;Port=5432;User Id=Chevauchee; Password=Chevauchee;Database=postgres; pooling=false");
        dbConnection.Open();
        IDbCommand createDB = dbConnection.CreateCommand();
        createDB.CommandText = "CREATE DATABASE chevauchee";
        createDB.ExecuteNonQuery();
        dbConnection.ChangeDatabase("chevauchee");
        IDbCommand addExtensions = dbConnection.CreateCommand();
        addExtensions.CommandText = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";";
        addExtensions.ExecuteNonQuery();
        IDbCommand createSchema = dbConnection.CreateCommand();
        createSchema.CommandText = "CREATE SCHEMA chevauchee; ";
        createSchema.ExecuteNonQuery();
        IDbCommand createTable = dbConnection.CreateCommand();
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
        foreignKey.ExecuteNonQuery();
    }

    public static void KillDatabase()
    {
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
