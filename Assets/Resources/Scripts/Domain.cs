using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Domain 
{

    public Guid id { get; protected set; }
    public string name { get; protected set; }
    public Guid character { get; protected set; }
    public Guid GetId() { return id; }
    private ResourceList m_resource;
    public ResourceList GetResource() { return m_resource; }
    public void GiveResource(resource givenResource, int givenAmount) {m_resource.Add(givenResource, givenAmount);}
    public void GiveResource(resource givenResource) {m_resource.Clear(givenResource);}
    public void TakeResource(resource givenResource, int givenAmount) {m_resource.Subtract(givenResource, givenAmount);}
    public void TakeResource(resource givenResource) { m_resource.Clear(givenResource); }
    public int GetResource(int givenInt) {return m_resource.GetAmount(ResourceList.ConvertInt(givenInt)); }
    public int GetResource(resource givenResource) { return m_resource.GetAmount(givenResource); }
    
    public Domain()
    {
        m_resource = new ResourceList();
    }
    public Domain(string givenName, Character givenCharacter) : this()
    {
        name = givenName;
        character = givenCharacter.id;
    }

    public Tile GetCapital()
    {
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            Tile capitalProperty = db.Tile.Single(t => t.domain == GetId());
            return capitalProperty;
        }
    }

    public void CountResource()
    {
        m_resource.Clear();
        m_resource.Add(GetProduction());
        m_resource.Subtract(GetMaintenance());
    }
    public ResourceList GetMaintenance()
    {
        ResourceList totalmaintenance = new ResourceList();
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            List<Tile> m_tile = db.Tile.Where(t => t.domain == GetId()).ToList();
            foreach (Tile currTile in m_tile)
            {
                Building[] currentBuilding = currTile.GetBuildings();
                for (int i = 0; i < currentBuilding.Length; i++)
                {
                    totalmaintenance.Add(currentBuilding[i].GetMaintenance());
                }
            }
        }
        return totalmaintenance;
    }
    public ResourceList GetProduction()
    {
        ResourceList totalproduction = new ResourceList();
        using (Database.DatabaseManager db = new Database.DatabaseManager())
        {
            List<Tile> m_tile = db.Tile.Where(t => t.domain == GetId()).ToList();
            foreach (Tile currTile in m_tile)
            {
                Building[] currentBuilding = currTile.GetBuildings();
                for (int i = 0; i < currentBuilding.Length; i++)
                {
                    if (currentBuilding[i] is Industrial)
                    {
                        totalproduction.Add(((Industrial)currentBuilding[i]).ProduceResource());
                    }
                }
            }
        }
        return totalproduction;
    }
    
    public void Grow(string building)
    {
        //formula for population growth
        //make any new cities that formed
        /*if (building == "Farm")
  {
      //for testing only
      m_building = TestMain.AddElement<Building>(m_building, new Farm());
  }
  else if (building == "LumberMill")
  {
      m_building = TestMain.AddElement<Building>(m_building, new LumberMill());
  }
  if(building == "City")
  {

  }*/
    }


    //public void SpreadCulture();

}



/*public void AddBuilding(string buildingType, int[] position)
{
    if (buildingType == "Farm")
    {
        //for testing only
        m_building = TestMain.AddElement<Building>(m_building, new Farm());
    }
    else if (buildingType == "LumberMill")
    {
        m_building = TestMain.AddElement<Building>(m_building, new LumberMill());
    }
    if(buildingType == "Capital")
    {
        m_building[0] = new City();
    }
}*/
