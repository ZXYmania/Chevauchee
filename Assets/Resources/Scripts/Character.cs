using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character
{
    public Character()
    {

    }
    public Character(string givenName)
    {
        id = Guid.NewGuid();
        name = givenName;
    }

    //the state that you are head of
    public Guid id { get; protected set; }
    public string name { get; protected set; }
    public string GetName() { return name; }
	// Use this for initialization
    public void Crown(string givenKingdom)
    {
        throw new NotImplementedException();
    }
    public void AddDomain(Domain givenDomain)
    {
        throw new NotImplementedException();
    }
    public void SwearFealty()
    {
       
    }

    public Domain[] GetDomain()
    {
        throw new NotImplementedException();
        //ResourceList[] duchyNetResource = new ResourceList[controlled.Length];
        //Debug.Log("Controlled " + controlled.GetSize());
        //return controlled.GetOrderedArray();
        //return duchyNetResource;
    }
    public bool RulesDomain(Guid givenDomain)
    {
        throw new NotImplementedException();
        /*Kingdom kingdom = KingdomDictionary.GetKingdom(m_state);
        if (kingdom != null)
        {
            return kingdom.ContainsDomain(givenDomain);
        }
        else
        {
            return ControlsDomain(givenDomain);
        }*/
    }
    public bool ControlsDomain(Guid givenDomain)
    {
        throw new NotImplementedException();
        //return controlled.Contains(givenDomain);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
