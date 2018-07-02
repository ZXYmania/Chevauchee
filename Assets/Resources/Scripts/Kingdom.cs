using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kingdom
{
    public enum Title
    {
        Emperor,
        King,
        Duke
    }
    public class Nobility
    {
        public Guid kingdom { get; protected set; }
        public Guid character { get; protected set; }
        public Title title { get; protected set; }
        Nobility() { }
        public Nobility(Kingdom givenKingdom, Character givenCharacter, Title givenTitle)
        {
            kingdom = givenKingdom.id;
            character = givenCharacter.id;
            title = givenTitle;
        }
    }
    public string GetName() { return name; }
    public Guid id;
    public string name { get; protected set; }
    ResourceList[] ImportsExports;
    // Use this for initialization
    public Kingdom(Character givenKing, string givenName)
    {
        id = Guid.NewGuid();
        name = givenName;
        Coronate(givenKing);
        //[0] = imports  [1] = exports
        ImportsExports = new ResourceList[2];
    }
    public List<Character> GetAllCharacters()
    {
        throw new NotImplementedException();
        /*Character[] allCharacter = new Character[0];
        TestMain.AddElement<Character>(ref allCharacter, emperor);
        TestMain.AddElement<Character>(ref allCharacter, king);
        TestMain.AddElement<Character>(ref allCharacter, duke);
        return allCharacter;*/
    }
    private void SetEmperor(Character givenEmperor)
    {
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            Nobility currentTitle = new Nobility(this, givenEmperor, Title.Emperor);
            db.SaveChanges();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Coronate(Character givenKing)
    {
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            Nobility currentTitle = new Nobility(this,givenKing, Title.King);
            db.SaveChanges();
        }
    }

    protected void AddDuke(Character givenDuke)
    {
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            Nobility currentTitle = new Nobility(this, givenDuke, Title.Duke);
            db.SaveChanges();
        }
    }

    public void AcceptFealty(Character givenCharacter)
    {
        givenCharacter.SwearFealty();
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            db.Nobility.Where(n => n.kingdom == id && n.character == givenCharacter.id);
            AddDuke(givenCharacter);
        }
    }

    public List<Domain> GetDomain()
    {
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            List<Domain> allDomain = (from d in db.Domain
                                      join c in db.Character
                                      on d.character equals c.id
                                      join n in db.Nobility
                                      on c.id equals n.character
                                      where n.kingdom == id
                                      select d).ToList();
            return allDomain;
        }
    }
    public bool ContainsDomain(Guid givenDomain)
    {
        List<Character> allCharacter = GetAllCharacters();
        foreach(Character currCharacter in allCharacter)
        {
            throw new NotImplementedException();

            /*if (allCharacter[i].ControlsDomain(givenDomain))
            {
                return true;
            }*/
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
 