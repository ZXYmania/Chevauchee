using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public partial class Database
{
    public class TileTransaction : Transaction
    {
        public static TileTransaction CreateTileTransaction()
        {
            return new TileTransaction(dbConnection);
        }

        public  TileTransaction(IDbConnection givenConnection) : base(givenConnection)
        {
            m_table = "chevauchee.tile";
            m_column = new string[4];
            m_column[0] = "x";
            m_column[1] = "y";
            m_column[2] = "terrain";
            m_column[3] = "domain_id";
        }

        public void AddNewTile(int givenX, int givenY, Tile.Terrain givenTerrain)
        {
            string tileColumn = "(" + givenX.ToString() + "," + givenY.ToString() + ",'" + (int)givenTerrain +"','"+Guid.Empty.ToString()+"')";
            AddNewItem(tileColumn);
        }

        protected override void AddNewItem(string givenItem)
        {
            if (CheckTouched(SQLType.Insert))
            {
                m_command.CommandText += ", ";
            }
            else
            {
                SetType(SQLType.Insert);
            }
            m_command.CommandText += givenItem;
        }

        public void SelectDomain(Guid givenDomain)
        {
            AddSelectCondition(m_column[3], givenDomain.ToString());
        }

        public void SelectBounds(Position[] bounds, bool hollow)
        {
            if(bounds.Length!=2 )
            {
                throw new KeyNotFoundException("The bounds given in select bounds was of an invalid length");
            }
            if (hollow)
            {

            }
            else
            {
                AddSelectCondition(m_column[0], bounds[0].x.ToString(), ">=");
                AddSelectCondition(m_column[0], bounds[1].x.ToString(), "<=");
                AddSelectCondition(m_column[1], bounds[0].y.ToString(), ">=");
                AddSelectCondition(m_column[1], bounds[1].y.ToString(), "<=");
                ExecuteSelect();
            }
        }

        protected override void AddSelectCondition(string columnName, string value, string operation = "=")
        {
            if (CheckTouched(SQLType.Select))
            {
                m_command.CommandText += " And ";
            }
            else
            {
                m_command.CommandText += "where ";
            }
            m_command.CommandText += columnName + operation + value;
        }

        public void UpdateItem(Tile givenItem)
        {
            Dictionary<string, string> givenfield = new Dictionary<string, string>();
            KeyValuePair<string, string> key = new KeyValuePair<string, string>("Position", "(" + givenItem.GetX() + ", " + givenItem.GetY() + ")"); //this is the key column
            givenfield.Add("Domain", givenItem.GetDomain().ToString());
            givenfield.Add("Terrain", ""+(int)givenItem.GetTerrain());
            UpdateItem(key, givenfield);
        }
        protected void UpdateItem(KeyValuePair<string, string> key, Dictionary<string, string> givenField)
        {

            m_command.CommandText += "set ";
            bool firstItem = true;
            foreach (KeyValuePair<string, string> item in givenField)
            {
                if (!firstItem)
                {
                    m_command.CommandText += ", ";
                }
                else { firstItem = true; }
                m_command.CommandText += item.Key + "=" + item.Value;
            }
            m_command.CommandText += "where " + key.Key + "=" + key.Value;
        }

        protected override void UpdateItem(string key, string givenfields)
        {
            if (CheckTouched(SQLType.Update))
            {
                m_command.CommandText += ", ";
            }
            throw new NotImplementedException();
        }

        public void DataReaderActive()
        {
            if (m_touched < 2)
            {
                throw new UnimplementTransactionError("Transaction was attempted to be read when it hadn't been executed");
            }
            if (m_dataReader.IsClosed)
            {
                throw new ReUsingTransactionError("Attempting to access a datareader that has finished");
            }
        }
        public bool GetNextRow()
        {
            DataReaderActive();
            bool itemExist = m_dataReader.Read();
            if (!itemExist)
            {
                m_dataReader.Close();
            }
            return itemExist;
        }
        public Position GetPosition()
        {
            DataReaderActive();
            return new Position( (int)m_dataReader[m_column[0]], (int)m_dataReader[m_column[1]]);
        }
        public Tile.Terrain GetTerrain()
        {
            DataReaderActive();
            return (Tile.Terrain)m_dataReader[m_column[2]];
        }
        public Guid GetDomain()
        {
            DataReaderActive();
            return (Guid)m_dataReader[m_column[3]];
        }
    }
}
