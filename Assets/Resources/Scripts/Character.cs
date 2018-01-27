using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character 
{
    //the state that you are head of
    string m_state;
    string m_name;
    public string GetName() { return m_name; }
    SearchAbleArray<string> controlled;
    SearchAbleArray<string> owned;
	// Use this for initialization
    public Character(string givenName)
    {
        controlled = new SearchAbleArray<string>();
        m_name = givenName;
    }
    public void Crown(string givenKingdom)
    {
        m_state = givenKingdom;
    }
    public Character(Domain givenDomain)
    {
        controlled = new SearchAbleArray<string>();
        controlled.Add(givenDomain.GetName());
    }
    public void AddDomain(Domain givenDomain)
    {
       controlled.Add(givenDomain.GetName());
    }
    public void SwearFealty()
    {
       
    }

    public string[] GetDomain()
    {
        //ResourceList[] duchyNetResource = new ResourceList[controlled.Length];
        Debug.Log("Controlled " + controlled.GetSize());
        return controlled.GetOrderedArray();
        //return duchyNetResource;
    }
    public bool RulesDomain(string givenDomain)
    {
        Kingdom kingdom = KingdomDictionary.GetKingdom(m_state);
        if (kingdom != null)
        {
            return kingdom.ContainsDomain(givenDomain);
        }
        else
        {
            return ControlsDomain(givenDomain);
        }
    }
    public bool ControlsDomain(string givenDomain)
    {
         return controlled.Contains(givenDomain);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
