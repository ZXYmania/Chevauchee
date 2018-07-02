using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TestMain : MonoBehaviour
{
    // Use this for initialization
    private static Camera[] m_camera;
    private static Canvas m_canvas;
    public static Domain environmentDomain { get; protected set; }
    public static Character environmentCharacter { get; protected set; }
    public static void MoveCamera(int givenX, int givenY)
    {
        m_camera[0].transform.position = new Vector3(givenX, givenY, m_camera[0].transform.position.z);
        m_canvas.transform.position = new Vector3(givenX, givenY, m_canvas.transform.position.z);
    }
    private static Player[] m_player;
    public static Camera GetCamera() { return m_camera[0]; }
    public static Canvas GetCanvas() { return m_canvas; }
    public static Player GetPlayer() { return m_player[0]; }
    static Map m_map;
    static public Map GetMap() { return m_map; }
	void Start ()
    {
        Debug.Log("Start");
        Initialise();

    }
    public void Initialise()
    {
       // Database serverDB = new Database();
        Debug.Log("test");
        m_camera = new Camera[] { gameObject.GetComponent<Camera>()};
        GameObject canvasObj = new GameObject();
        m_canvas = canvasObj.AddComponent<Canvas>();
        m_canvas.transform.position = new Vector3(m_camera[0].transform.position.x, m_camera[0].transform.position.y, -3);
        TextureController.Initialise();
        CreateGameState();
    }
    public void CreateGameState()
    { 
        m_map = new Map();
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            if (db.Database.EnsureCreated())
            {
                environmentCharacter = new Character("Environment");
                environmentDomain = new Domain("Environment", environmentCharacter);
                db.Character.Add(environmentCharacter);
                db.Domain.Add(environmentDomain);
                db.SaveChanges();
                m_map.GenerateMap();
            }
            else
            {
                Debug.Log("Map has NOT been loaded");
            }
        }
        m_player = new Player[] { gameObject.AddComponent<Player>() };
        Character firstPlayer = new Character("Trajan");
        m_player[0].Initialise(firstPlayer);
        /*CharacterDictionary.AddCharacter("Hannibal");
        CharacterDictionary.AddCharacter("Trajan");
        CharacterDictionary.AddCharacter("Pompeii");
        CharacterDictionary.AddCharacter("Caeser");
        CharacterDictionary.AddCharacter("Darius");

        KingdomDictionary.AddKingdom("Persia", "Darius");
        KingdomDictionary.AddKingdom("Rome", "Trajan");
        Kingdom rome = KingdomDictionary.GetKingdom("Rome");
        rome.AcceptFealty(CharacterDictionary.GetCharacter("Pompeii"));
        rome.AcceptFealty(CharacterDictionary.GetCharacter("Caeser"));
        KingdomDictionary.AddKingdom("Carthage", "Hannibal");

        DomainDictionary.AddDomain("Carthage", CharacterDictionary.GetCharacter("Hannibal"));
        DomainDictionary.AddDomain("Ravenna", CharacterDictionary.GetCharacter("Trajan"));
        DomainDictionary.AddDomain("Milan", CharacterDictionary.GetCharacter("Caeser"));
        DomainDictionary.AddDomain("Ostia", CharacterDictionary.GetCharacter("Trajan"));
        DomainDictionary.AddDomain("Persia", CharacterDictionary.GetCharacter("Darius"));       

        Trade.Initialise();
        try
        {
            //Trade.StartTradeKingdom("Rome");
        }
        catch (TileOutofBoundsException e)
        {
            Debug.Log("Domain trade is unimplemented");
        }
        m_player = new Player[] { gameObject.AddComponent<Player>() };
        m_player[0].Initialise(CharacterDictionary.GetCharacter("Trajan"));*/
    }


    public static GameObject CreateObject(string objectName, int[] position)
    {
       return Instantiate(Resources.Load(objectName), new Vector3(position[0], position[1], 0), Quaternion.identity) as GameObject;
    }
     
    public static Sprite LoadTexture(string givenPath) { return Resources.Load(givenPath, typeof(Sprite)) as Sprite; }
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            //Trade.StartTradeKingdom("Rome");
        }
    }

    public static void AddElement<T>(ref T[] givenArray, T givenElement)
    {
        if (givenElement != null)
        {
            T[] temp = new T[givenArray.Length + 1];
            for (int i = 0; i < temp.Length - 1; i++)
            {
                temp[i] = givenArray[i];
            }
            temp[temp.Length - 1] = givenElement;
            //givenArray = temp;
            givenArray = temp;
        }
    }
    public static void AddElement<T>(ref T[] givenArray, T[] givenElement)
    {
        if (givenElement != null)
        {
            T[] temp = new T[givenArray.Length + givenElement.Length];
            givenArray.CopyTo(temp, 0);
            givenElement.CopyTo(temp, givenArray.Length);
           givenArray = temp;
        }
    }
   public static void RemoveElement<T>(ref T[] givenArray, T givenElement)
    {
        T[] temp = new T[givenArray.Length - 1];
        int foundItem = 0;
        for (int i =0; i < givenArray.Length; i++)
        {
            if(!givenArray[i].Equals(givenElement))
            {
                temp[i - foundItem] = givenArray[i];
            }
            else
            {
                foundItem++;
                if(foundItem > 1)
                {
                    Debug.Log("Multiple Items found " + givenArray[i] + " ," + givenElement);
                }
            }
        }
        givenArray = temp;
    }



    public static void QuickSort<T>(float[] givenFloat, ref T[] givenT)
    {
        float[] tempArray;
        T[] tempT;
        if (givenFloat.Length > 1 && givenFloat.Length == givenT.Length)
        {
            int[] pivot = { (int)Mathf.Floor(givenFloat.Length / 2), 0, 1 };
            tempArray = new float[givenFloat.Length];
            tempT = new T[givenFloat.Length];
            for (int i = 0; i < givenFloat.Length; i++)
            {
                if (i != pivot[0])
                {
                    if (givenFloat[i] < givenFloat[pivot[0]])
                    {
                        //Debug.Log("iteration + " + pivot[1] + " :" + givenFloat[i].GetNames());
                        tempArray[pivot[1]] = givenFloat[i];
                        tempT[pivot[1]] = givenT[i];
                        pivot[1]++;
                    }
                    else
                    {
                        //Debug.Log("if bigger iteration " + (tempArray.Length - pivot[2]) + ": " + givenFloat[i].GetNames());
                        tempArray[tempArray.Length - pivot[2]] = givenFloat[i];
                        tempT[tempArray.Length - pivot[2]] = givenT[i];
                        pivot[2]++;
                    }
                }
            }

            //Create the two sub arrays
            tempArray[pivot[1]] = givenFloat[pivot[0]];
            tempT[pivot[1]] = givenT[pivot[0]];
            float[] small = new float[pivot[1]];
            T[] smallT = new T[pivot[1]];
            float[] big = new float[pivot[2] - 1];
            T[] bigT = new T[pivot[2] - 1];

            //Debug.Log(givepivot[0]);
            for (int i = 0; i < small.Length; i++)
            {
                small[i] = tempArray[i];
                smallT[i] = tempT[i];
            }
            QuickSort(small, ref smallT);
            smallT.CopyTo(tempT, 0);
            for (int i = 0; i < big.Length; i++)
            {
                big[i] = tempArray[pivot[1] + 1 + i];
                bigT[i] = tempT[pivot[1] + 1 + i];
            }
            QuickSort(big, ref bigT);
            bigT.CopyTo(tempT, pivot[1] + 1);
            givenT = tempT;
        }
        
    }
    void OnApplicationQuit()
    {
        Database.KillDatabase();
    }
}


