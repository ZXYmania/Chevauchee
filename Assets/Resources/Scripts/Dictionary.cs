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

public static class KingdomDictionary : object
{
    static int Length;
    public static int GetLength() { return Length; }
    static Dictionary<string, Kingdom> AllKingdom;
    public static Dictionary<string, Kingdom> GetDictionary() { return AllKingdom; }
    public static Kingdom GetKingdom(string givenKingdom)
    {
        if (givenKingdom != null)
        {
            if (AllKingdom.ContainsKey(givenKingdom))
            {
                return AllKingdom[givenKingdom];
            }
            else
            {
                throw new NonExistantKeyError(givenKingdom + " Does not exist.");
            }
        }
        else
        {
            return null;
        }
    }
    public static void AddKingdom(string kingdomName, string characterName)
    {
        Kingdom newKingdom = new Kingdom(CharacterDictionary.GetCharacter(characterName), kingdomName);
        AllKingdom.Add(kingdomName, newKingdom);
        Length++;
    }
    public static bool Contains(string givenKingdom) { return AllKingdom.ContainsKey(givenKingdom); }
    public static void AddKingdom(string kingdomName, Character character)
    {
        Kingdom newKingdom = new Kingdom(character, kingdomName);
        AllKingdom.Add(kingdomName, newKingdom);
        Length++;
    }

    // Use this for initialization
    public static void Initialise()
    {
        AllKingdom = new Dictionary<string, Kingdom>();
        Length = 0;
    }
	public static bool Save()
    {
        return false;
    }
    public static bool Load()
    {
        return false;
    }
}



public static class DomainDictionary : object
{

    static int Length;
    public static int GetLength() { return Length; }
    static Dictionary<string, Domain> AllDomain;
    public static Dictionary<string, Domain> GetDictionary() { return AllDomain; }
    public static Domain GetDomain(string givenDomain)
    {
        if (givenDomain != null)
        {
            if (AllDomain.ContainsKey(givenDomain))
            {
                return AllDomain[givenDomain];
            }
            else
            {
                throw new NonExistantKeyError(givenDomain + " Does not exist.");
            }
        }
        else
        {
            return null;
        }
    }
    public static bool Contains(string givenDomain) { return AllDomain.ContainsKey(givenDomain); }

    public static void AddDomain(string domainName, Character givenCharacter)
    {
        Domain newDomain = new Domain();
        givenCharacter.AddDomain(newDomain);
        AllDomain.Add(domainName, newDomain);
        Length++;
    }

    // Use this for initialization
    public static void Initialise()
    {
        AllDomain = new Dictionary<string, Domain>();
        Length = 0;
    }

}

public static class CharacterDictionary : object
{
    static int Length;
    public static int GetLength() { return Length; }
    static Dictionary<string, Character> AllCharacter;
    public static Dictionary<string, Character> GetDictionary() { return AllCharacter; }
    public static Character GetCharacter(string givenCharacter)
    {
        if (givenCharacter != null)
        {
            if (AllCharacter.ContainsKey(givenCharacter))
            {
                return AllCharacter[givenCharacter];
            }
            else
            {
                throw new NonExistantKeyError(givenCharacter + " Does not exist.");
            }
        }
        else
        {
            return null;
        }
    }
    public static bool Contains(string givenCharacter) { return AllCharacter.ContainsKey(givenCharacter); }
    public static void AddCharacter(string characterName)
    {
        Character newCharacter = new Character(characterName);
        AllCharacter.Add(characterName, newCharacter);
        Length++;
    }
    public static void AddCharacter(string characterName, Domain givenDomain)
    {
        Character newCharacter = new Character(characterName);
        AllCharacter.Add(characterName, newCharacter);
        Length++;
    }

    // Use this for initialization
    public static void Initialise()
    {
        AllCharacter = new Dictionary<string, Character>();
        Length = 0;
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
    int amountofRows;
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
/*public class IndexedDictionary<T, U>
{
    protected Dictionary<T, int> m_dictionary;
    protected U[] m_array;
    public void Add(T givenT, U givenU)
    {
        m_dictionary.Add(givenT, m_array.Length);
        U[] temp = new U[m_array.Length + 1];
        m_array.CopyTo(temp,0);
        temp[m_array.Length] = givenU;
        m_array = temp;
    }
    public void Remove(T givenT)
    {
        int tempIndex = m_dictionary[givenT];
        m_dictionary.Remove(givenT);
        U[] tempArray = new U[m_array.Length-1];
        m_array[tempIndex] = default(U);
        tempIndex = 0;
        for(int i = 0; i < m_array.Length; i++)
        {
            if (!EqualityComparer<U>.Default.Equals(m_array[i] , default(U)))
            {
                tempArray[tempIndex] = m_array[i];
                tempIndex++;
            }
        }
        m_array = tempArray;
    }
    public void Remove(T[] givenT)
    {
        int tempIndex = 0;
        U[] tempArray = null;
        U nullU = default(U);
        for (int i = 0; i < givenT.Length; i++)
        {
            tempIndex = m_dictionary[givenT[i]];
            m_dictionary.Remove(givenT[i]);
            tempArray = new U[m_array.Length - 1];
            m_array[tempIndex] = nullU;
        }
        tempIndex = 0;
        for (int i = 0; i < m_array.Length; i++)
        {
            if (!IComparable<U>.Equals(m_array[i], nullU))
            {
                tempArray[tempIndex] = m_array[i];
                Replace(m_dictionary[i],)
                tempIndex++;
            }
        }
        m_array = tempArray;
    }
    private void RemoveFromDictionary(int givenIndex)
    {
        foreach (KeyValuePair<T, int> currKey in m_dictionary)
        {
            if (currKey.Value == givenIndex)
            {
                m_dictionary.Remove(currKey.Key);
                return;
            }
        }
    }
    public void Replace(IndexedDictionary<T,U> givenDict)
    {
        givenDict.GetOrderedList().CopyTo(m_array, 0);
        foreach(T item in givenDict.GetIterable())
        {
           RemoveFromDictionary(givenDict.GetIndex(item));
           Add(item, givenDict.GetItem(item));
        }
    }
    public int GetSize() { return m_array.Length; }
    public U[] GetOrderedList() { U[] temp = m_array; return temp; }
    private Dictionary<T, int>.KeyCollection GetIterable() { return m_dictionary.Keys; }
    public bool Contains(T givenT) { return m_dictionary.ContainsKey(givenT); }
    public U GetItem(int i) { return m_array[i]; }
    public U GetItem(T givenT) { return m_array[m_dictionary[givenT]]; }
    public int GetIndex(T givenT) { return m_dictionary[givenT]; }
    public U GetLast() { return m_array[m_array.Length - 1]; }

}*/

/*public abstract class My_Dictionary<T> : object
{
    protected Dictionary<string, T> m_list;
    public Dictionary<string, T> GetDictionary() { return m_list; }
    public T GetElement(string givenKey) { return m_list[givenKey]; }

    //public abstract void AddElement(string key);

    // Use this for initialization
    public abstract void Initialise();
} */
