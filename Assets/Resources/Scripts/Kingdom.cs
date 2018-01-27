using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kingdom
{
    public string GetName() { return m_name; }
    private string m_name;
    Character emperor;
    Character[] king;
    Character[] duke;
    ResourceList[] ImportsExports;
    // Use this for initialization
    public Kingdom(Character givenKing, string givenName)
    {
        m_name = givenName;
        king = new Character[0];
        Coronate(givenKing);
        duke = new Character[0];
        //[0] = imports  [1] = exports
        ImportsExports = new ResourceList[2];
    }
    public Character[] GetAllCharacters()
    {
        Character[] allCharacter = new Character[0];
        TestMain.AddElement<Character>(ref allCharacter, emperor);
        TestMain.AddElement<Character>(ref allCharacter, king);
        TestMain.AddElement<Character>(ref allCharacter, duke);
        return allCharacter;
    }
    private void SetEmperor(Character givenemp)
    {
        emperor = givenemp;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Coronate(Character givenKing)
    {
        TestMain.AddElement<Character>(ref king, givenKing);
        givenKing.Crown(m_name);
    }

    public void AcceptFealty(Character givenCharacter)
    {
        givenCharacter.SwearFealty();
        Character[] allCharacter = GetAllCharacters();
        for(int i = 0; i < allCharacter.Length; i++)
        {
            if(givenCharacter.GetName() == allCharacter[i].GetName())
            {
                return;
            }
        }
        AddDuke(givenCharacter);
    }
    protected void AddDuke(Character givenCharacter)
    {
        TestMain.AddElement<Character>(ref duke, givenCharacter);
    }
    public string[] GetDomain()
    {
        string[] realmDomain = new string[0];
        if (emperor != null)
        {
            TestMain.AddElement<string>(ref realmDomain, emperor.GetDomain());
        }
        string[] kingTally = new string[0];
        for (int i = 0; i < king.Length; i++)
        {
            TestMain.AddElement<string>(ref kingTally, king[i].GetDomain());
        }
        TestMain.AddElement<string>(ref realmDomain, kingTally);
        string[] dukeTally = new string[0];
        for (int i = 0; i < duke.Length; i++)
        {
            TestMain.AddElement<string>(ref dukeTally, duke[i].GetDomain());
        }
        TestMain.AddElement<string>(ref realmDomain, dukeTally);
        return realmDomain;
    }
    public bool ContainsDomain(string givenDomain)
    {
        Character[] allCharacter = GetAllCharacters();
        for(int i = 0; i < allCharacter.Length; i++)
        {
            if(allCharacter[i].ControlsDomain(givenDomain))
            {
                return true;
            }
        }
        return false;
    }
    //Trade ---------------------------------------------------------------------------
    public ResourceList SetImportsExports()
    {
        //ResourceList[] resourceTally = DoDomesticTrade();
        //Sort city pairs by proximity to one another
        //Have the closest cities trade with each other as much as possible
        //send the remains to the nearest port
        //return the imports and exports
        //----------------------------------------------------------------------
        /*ImportsExports[0] = new ResourceList();
        ImportsExports[1] = new ResourceList();
        ResourceList totalMaintenance = GetMaintenance();
        ResourceList totalProduction = GetProduction();
        for (int i = 0; i < ResourceList.AMOUNT_OF_RESOURCES; i++)
        {
            ImportsExports[1].Add( ResourceList.ConvertInt(i), totalProduction.GetAmount(ResourceList.ConvertInt(i)) - totalMaintenance.GetAmount(ResourceList.ConvertInt(i)));
            if (ImportsExports[1].GetAmount(ResourceList.ConvertInt(i)) < 0)
            {
                ImportsExports[1].Clear(ResourceList.ConvertInt(i));
                ImportsExports[0].Add(ResourceList.ConvertInt(i), totalProduction.GetAmount(ResourceList.ConvertInt(i)) - totalMaintenance.GetAmount(ResourceList.ConvertInt(i)));
            }
            totalProduction.Subtract(ResourceList.ConvertInt(i), totalMaintenance.GetAmount(ResourceList.ConvertInt(i)));
        }*/
        //ResourceList exportsimports = exports;
        ResourceList sumResource = new ResourceList();
        return sumResource;
    }
    public ResourceList GetExports() { return ImportsExports[1]; }
    public ResourceList GetImports() { return ImportsExports[0]; }

    public void PayExports(int[] price)
    {
        //Every domain gets fully paid for their resources, 
        //The cost for any resource left over is taken out of the kings coffers  
    }
    public void SupplyImports()
    {

    }
    // 
    //------------------------------------------------------------------------------------
   
    

}
    /*public ResourceList GetProduction()
    {
        ResourceList totalProduction = new ResourceList();
        //for each character in the kingdom
        if(emperor != null)
        {
           totalProduction.Add(emperor.GetProduction());
        }
        for(int i = 0; i < king.Length; i++)
        {
            totalProduction.Add(king[i].GetProduction());
        }
        for(int i = 0; i < duke.Length; i++)
        {
            totalProduction.Add(duke[i].GetProduction());
        }
        return totalProduction;
    }
    public ResourceList GetMaintenance()
    {
        ResourceList totalMaintenance = new ResourceList();
        //for each character in the kingdom
        if (emperor != null)
        {
            totalMaintenance.Add(emperor.GetMaintenance());
        }
        for (int i = 0; i < king.Length; i++)
        {
            totalMaintenance.Add(king[i].GetMaintenance());
        }
        for (int i = 0; i < duke.Length; i++)
        {
            totalMaintenance.Add(duke[i].GetMaintenance());
        }
        return totalMaintenance;
    }*/
 