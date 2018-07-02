using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class NonExistantKeyError : Exception
{
    public NonExistantKeyError(string message) : base(message)
    {

    }
}


public static class TradeNetworkDictionary : object
{
    static int Length;
    public static int GetLength() { return Length; }
    static Dictionary<string, TradeNetwork> AllTradeNetwork;
    //public static Dictionary<string, Character> GetDictionary() { return AllCharacter; }
    public static TradeNetwork GetTradeNetwork(string givenTradeNetwork)
    {
        if (givenTradeNetwork != null)
        {
            if (AllTradeNetwork.ContainsKey(givenTradeNetwork))
            {
                return AllTradeNetwork[givenTradeNetwork];
            }
            else
            {
                throw new NonExistantKeyError(givenTradeNetwork + " Does not exist.");
            }
        }
        else
        {
            return null;
        }
    }
    public static void AddTradeNetwork(string networkName)
    {
        TradeNetwork newTradeNetwork = new TradeNetwork();
        AllTradeNetwork.Add(networkName, newTradeNetwork);
        Length++;
    }
    // Use this for initialization
    public static void Initialise()
    {
        AllTradeNetwork = new Dictionary<string, TradeNetwork>();
        Length = 0;
    }
}

public class TileArray
{

    Tile[][] m_array;
    public int GetAmountofRows() {return m_array.Length;}
    public TileArray() 
    {
        m_array = new Tile[0][];
    }
    public Tile[] IterateThroughRows(int index)
    {
        return m_array[index];
    }
    public Tile[] GetRow(int givenX)
    {
        for(int i = 0; i < m_array.Length; i++)
        {
            if(m_array[i][0].GetX() == givenX)
            {
                return m_array[i];
            }
        }
        return null;
    }
    public Tile GetTile(Position givenPosition)
    {
        if (TestMain.GetMap().GetTile(givenPosition) != null)
        {
            for (int i = 0; i < m_array.Length; i++)
            {
                if (m_array[i][0].GetX() == givenPosition.x)
                {
                    for(int j = 0; j < m_array[i].Length; j++)
                    {
                        if(m_array[i][j].GetPosition().Equals(givenPosition))
                        {
                            return m_array[i][j];
                        }
                    }
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }
    public bool Contains(Tile givenTile)
    {
        for (int i = 0; i < m_array.Length; i++)
        {
            if (m_array[i][0].GetX() == givenTile.GetX())
            {
                for (int j = 0; j < m_array[i].Length; j++)
                {
                    if (m_array[i][j].GetX() == givenTile.GetX()
                        && m_array[i][j].GetY() == givenTile.GetY())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void AddTile(Tile givenTile)
    {
        if (!Contains(givenTile))
        {
            if (m_array.Length != 0)
            {
                int currYIndex = 0;
                Tile[][] tempArray = new Tile[m_array.Length + 1][];
                for (int i = 0; i < m_array.Length; i++)
                {
                    Tile[] tempRow = null;
                    if (givenTile.GetX() == m_array[i][0].GetX())
                    {
                        int currXIndex = 0;
                        tempRow = new Tile[m_array[i].Length + 1];
                        for (int j = 0; j < m_array[i].Length; j++)
                        {
                            if (givenTile.GetY() < m_array[i][j].GetY() && currXIndex == j)
                            {
                                //add in the tile to this row of the temp array
                                tempRow[currXIndex] = givenTile;
                                currXIndex++;
                            }
                            tempRow[currXIndex] = m_array[i][j];
                            currXIndex++;
                        }
                        if (currXIndex < tempRow.Length)
                        {
                            tempRow[currXIndex] = givenTile;
                        }
                        m_array[i] = tempRow;
                        return;
                    }
                    else if (givenTile.GetX() < m_array[i][0].GetX() && currYIndex == i)
                    {
                        tempArray[currYIndex] = new Tile[1];
                        tempArray[currYIndex][0] = givenTile;
                        currYIndex++;

                    }
                    tempArray[currYIndex] = m_array[i];
                    currYIndex++;
                }
                if (currYIndex < tempArray.Length)
                {
                    tempArray[currYIndex] = new Tile[1];
                    tempArray[currYIndex][0] = givenTile;
                }
                m_array = tempArray;
            }
            else
            {
                m_array = new Tile[1][];
                m_array[0] = new Tile[1];
                m_array[0][0] = givenTile;
            }
        }
    }
}

public class SearchAbleArray<T>
{
    protected Dictionary<T, int> m_dictionary;
    T lastItem;
    public SearchAbleArray()
    {
        m_dictionary = new Dictionary<T, int>();
        //Debug.Log("dictionary size "+m_dictionary.Count);
    }
    public int GetSize() { return m_dictionary.Count; }
    public T GetLastItem() { return lastItem;}
    public void Add(T givenT){ if (!m_dictionary.ContainsKey(givenT)){ m_dictionary.Add(givenT, m_dictionary.Count); lastItem = givenT; GetOrderedArray(); } }
    public void Add(SearchAbleArray<T> givenDictionary)
    {
        T[] givenArray = givenDictionary.GetOrderedArray();
        for(int i = 0; i < givenArray.Length; i++)
        {
            Add(givenArray[i]);
        }
    }
    // private void Add(T givenT, int givenIndex) { m_dictionary.Add(givenT, givenIndex); }
    public bool Contains(T givenT) { return m_dictionary.ContainsKey(givenT);}
    public void RemoveLast()
    {
        //if (m_dictionary.ContainsKey(givenT))
        {
            Dictionary<T, int> tempDict = new Dictionary<T, int>();
            T[] myArray = GetOrderedArray();
            int i;
            for (i = 0; i < myArray.Length-1; i++)
            {
               // if(!IComparable.Equals(myArray[i], givenT))
               // {
                    tempDict.Add(myArray[i], i);
                    lastItem = myArray[i];
               // }
            }

            m_dictionary = tempDict;
        }
    }
    public void Remove(T givenT)
    {
        if (givenT != null)
        {
            if (m_dictionary.ContainsKey(givenT))
            {
                Dictionary<T, int> tempDict = new Dictionary<T, int>();
                T[] myArray = GetOrderedArray();
                int index = 0;
                for (int i = 0; i < myArray.Length; i++)
                {
                    if (!IComparable.Equals(myArray[i], givenT))
                    {
                        tempDict.Add(myArray[i], index);
                        lastItem = myArray[i];
                        index++;
                    }
                    else
                    {
                        if(i == myArray.Length-1 && i > 1)
                        {
                            lastItem = myArray[i - 1];
                        }
                    }
                }
                m_dictionary = tempDict;
            }
        }
    }
    public void Replace(SearchAbleArray<T> givenDictionary)
    {
        if (m_dictionary.ContainsKey(givenDictionary.GetLastItem()))
        {
            T[] givenArray = givenDictionary.GetOrderedArray();
            Dictionary<T, int> tempDict = new Dictionary<T, int>();
            T[] myarray = GetOrderedArray();
 
            bool add = true;
            int count = 0;
            for (int i = 0; i < myarray.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(myarray[i], givenArray[0]))
                {
                    add = false;
                    for (int j= 0; j < givenArray.Length; j++)
                    {
                        tempDict.Add(givenArray[j], count++);
                    }
                }
                if (add) { tempDict.Add(myarray[i], count++); lastItem = myarray[i]; }
                if (EqualityComparer<T>.Default.Equals( myarray[i], givenDictionary.GetLastItem()))
                {
                    add = true;
                }
            }
            m_dictionary = tempDict;
        }
        else
        {
            Debug.Log(givenDictionary.GetLastItem() + "was not found when replacing");
            T[] givenArray = givenDictionary.GetOrderedArray();
            Dictionary<T, int> tempDict = new Dictionary<T, int>();
            for (int i = 0; i < givenArray.Length; i++)
            {
                    if (i == givenArray.Length - 1)
                    {
                        lastItem = givenArray[i];
                    }
                    tempDict.Add(givenArray[i], i);
            }
            m_dictionary = tempDict;
            Debug.Log(ToString());
        }
    }
    public T GetIndex(int givenIndex)
    {
        if (givenIndex < GetSize() && givenIndex >= 0)
        {
            T[] tempArray = GetOrderedArray();
            return tempArray[givenIndex];
        }
        Debug.Log("Index out of bounds "+givenIndex);
        return default(T);
    }
    public T[] GetOrderedArray()
    {
        T[] m_array = new T[m_dictionary.Count];
        foreach(KeyValuePair<T, int> item in m_dictionary)
        {
            m_array[item.Value] = item.Key;
        }
        return m_array;
    }
    public T[] GetOrderedArray(bool test)
    {
        T[] m_array = new T[m_dictionary.Count];
        foreach (KeyValuePair<T, int> item in m_dictionary)
        {
            Debug.Log(item.Value + ": " + item.Key);
            m_array[item.Value] = item.Key;
        }
        return m_array;
    }
    public override string ToString()
    {
        String output = null;
        foreach (KeyValuePair<T, int> item in m_dictionary)
        {
            output += item.ToString();
        }
        return output;
    }
}
