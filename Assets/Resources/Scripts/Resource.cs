using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum resource
{
    //strategic
    clay,
    iron,
    steel,
    wheat,
    wood,
    wool,

    //luxury
    dye,
    fur,
    gold,
    salt,
    // silver,
}

public class ResourceList
{
    string owner;
    public static int AMOUNT_OF_RESOURCES = 10;
    public static int STRATEGIC_INDEX;
    public static int LUXURY_INDEX;
    int[] m_resourceList = new int[AMOUNT_OF_RESOURCES];
    public ResourceList()
    {
        for (int i = 0; i < AMOUNT_OF_RESOURCES; i++)
        {
            m_resourceList[i] = 0;
        }
    }
    public static int ConvertResource(resource givenResource) { return (int)givenResource; }
    public static resource ConvertInt(int givenInt) { return (resource)givenInt; }
    public int GetAmount(resource givenResource) { return m_resourceList[(int)givenResource]; }
    protected void SetAmount(resource givenResource, int givenAmount) { m_resourceList[(int)givenResource] = givenAmount; }
    public void Add(resource givenResource, int givenAmount) { m_resourceList[(int)givenResource] += givenAmount; }
    public void Add(ResourceList givenResourceList)
    {
        for (int i = 0; i < AMOUNT_OF_RESOURCES; i++)
        {
            int value = givenResourceList.GetAmount(ConvertInt(i));
            m_resourceList[i] += value;
        }
    }
    public void Subtract(resource givenResource, int givenAmount) { m_resourceList[ConvertResource(givenResource)] -= givenAmount; }
    public void Subtract(ResourceList givenResourceList)
    {
        for (int i = 0; i < AMOUNT_OF_RESOURCES; i++)
        {
            int value = givenResourceList.GetAmount(ConvertInt(i));
            m_resourceList[i] -= value;
        }
    }
    public void Clear(resource givenResource) { m_resourceList[(int)givenResource] = 0;}
    public void Clear()
    {
        for(int i = 0; i < AMOUNT_OF_RESOURCES; i++)
        {
            m_resourceList[i] = 0;
        }
    }
    //public void DeleteList() { m_resourceList.Clear(); }
    public override string ToString()
    {
        string result = "";
        for(int i = 0; i < m_resourceList.Length; i++)
        {
            if (m_resourceList[i] != 0)
            {
                result += ConvertInt(i) + " " + m_resourceList[i] + ", ";
            }
        }
        return result;
    }
}

/*public class Resource : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}*/
