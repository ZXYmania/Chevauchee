using System.Collections;
using System.Collections.Generic;
using System;
using Npgsql;
using System.Data;
using UnityEngine;
public partial class Database
{
    public enum SQLType
    {
        Select,
        Insert,
        Update
    }

    public class UnfinishedTransactionException : System.Exception
    {
        public UnfinishedTransactionException(string message) : base(message)
        {

        }
    }
    public class UnimplementTransactionError : System.Exception
    {
        public UnimplementTransactionError(string message) : base(message)
        {

        }
    }

    public class WrongTransactionTypeError : System.Exception
    {
        public WrongTransactionTypeError(string message) : base(message)
        {

        }
    }

    public class ReUsingTransactionError : System.Exception
    {
        public ReUsingTransactionError(string message) : base(message)
        {

        }
    }

    public abstract class Transaction
    {
        protected abstract void AddNewItem(string givenItem);
        protected abstract void AddSelectCondition(string columnName, string value, string operation);
        protected abstract void UpdateItem(string key, string givenfields);
        protected IDbCommand m_command;
        protected SQLType m_type;
        protected IDataReader m_dataReader;
        protected string[] m_column; //column name, valuetype
        protected string m_table;
        protected int m_touched;
        protected void SetType(SQLType givenType)
        {
            m_type = givenType;
            switch (m_type)
            {
                case SQLType.Select:
                    m_command.CommandText = "select * from "+m_table+" ";
                    break;
                case SQLType.Update:
                    m_command.CommandText = "update "+m_table+" ";
                    break;
                case SQLType.Insert:
                        m_command.CommandText = "insert into " + m_table + " (";
                        for(int i =0; i < m_column.Length; i++)
                        {
                            if (i!=0)
                            {
                                m_command.CommandText += ", ";
                            }
                            m_command.CommandText += m_column[i];
                        }
                        m_command.CommandText += ") VALUES ";
                   break;
            }
        }
        public SQLType GetSQLType() { return m_type; }
        protected Transaction(IDbConnection givenConnection)
        {
            m_command = givenConnection.CreateCommand();
            m_touched = 0;
        }
        ~Transaction()
        {
            if (m_command.CommandText != "finished")
            {
                throw new UnfinishedTransactionException(m_command.CommandText);
            }
        }
        public void ExecuteCommand()
        {
            if (m_touched != 1) { throw new UnimplementTransactionError(this.ToString()); }
            if (m_type != SQLType.Select)
            {
                m_command.CommandText += ";";
                m_command.ExecuteNonQuery();
                m_command.CommandText = "finished";
                m_touched = 2;
            }
            else
            {
                throw new WrongTransactionTypeError("tried to execute a non query while transaction type is " + m_type);
            }
        }
        public void ExecuteSelect()
        {
            if (m_touched != 1) { throw new UnimplementTransactionError(this.ToString()); }
            if (m_type == SQLType.Select)
            {
                m_command.CommandText += ";";
                m_dataReader = m_command.ExecuteReader();
                m_command.CommandText = "finished";
                m_touched = 2;
            }
            else
            {
                throw new WrongTransactionTypeError("tried to execute a query while transaction type is " + m_type);
            }
        }

        public bool CheckTouched(SQLType givenType)
        {
            switch (m_touched)
            {
                case 0:
                    m_touched = 1;
                    SetType(givenType);
                    return false;
                case 1:
                    if (m_type != givenType) { throw new WrongTransactionTypeError("tried to execute a query while transaction type is " + m_type); }
                    return true;
                default: throw new ReUsingTransactionError(this.ToString());
            }
        }
    }
}
