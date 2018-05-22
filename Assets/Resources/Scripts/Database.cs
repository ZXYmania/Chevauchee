using System;
using System.Collections;
using System.Collections.Generic;
//using Mono.Data.Sqlite;
using System.Data;
using Npgsql;
using UnityEngine;

public class Database
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
        IDbCommand createSchema = dbConnection.CreateCommand();
        createSchema.CommandText = "CREATE SCHEMA chevauchee; ";
        createSchema.ExecuteNonQuery();
        IDbCommand createTable = dbConnection.CreateCommand();
        createTable.CommandText = "Create TABLE chevauchee.Tile(X integer NOT NULL, Y integer NOT NULL, DomainId uuid, Terrain integer, CONSTRAINT position PRIMARY KEY(X, Y));";
        createTable.ExecuteNonQuery();
    }

    public static void KillDatabase()
    {
        dbConnection.Close();
    }
    // Use this for initialization
    public static void StartDatabase()
    {
        DatabaseCreationCommand();
 
    }
}
