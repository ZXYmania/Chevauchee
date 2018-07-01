using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domain 
{
    public class DomainProperty
    {
        public Guid id;
        public string name;
        public Guid character;
    }
    //A list of all buildings controlled by the domain
    Tile[] m_tile;
    //m_building[0] is the capital.
    public Tile GetCapital() { if (m_tile.Length < 1) { Debug.Log(GetId());return null; } else { return m_tile[0]; } }
    Tile[] m_occupyingbuilding;
    DomainProperty m_properties;
    public Guid GetId() { return m_properties.id; }
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
        m_properties = new DomainProperty();
        m_properties.id = Guid.NewGuid();
        m_properties.name = "cool";
        m_properties.character = Guid.NewGuid();
        m_resource = new ResourceList();
        m_tile = new Tile[0];
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
        for (int i = 0; i < m_tile.Length; i++)
        {
            Building[] currentBuilding = m_tile[i].GetBuildings();
            for (int j = 0; j < currentBuilding.Length; j++)
            {
                totalmaintenance.Add(currentBuilding[j].GetMaintenance());
            }
        }
        return totalmaintenance;
    }
    public ResourceList GetProduction()
    {
        ResourceList totalproduction = new ResourceList();
        for (int i = 0; i < m_tile.Length; i++)
        {
            Building[] currentBuilding = m_tile[i].GetBuildings();
            for (int j = 0; j < currentBuilding.Length; j++ )
            {
                if (currentBuilding[j] is Industrial)
                {
                    totalproduction.Add(((Industrial)currentBuilding[j]).ProduceResource());
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

    public void AddBuilding(Position givenBuilding)
    {
        Debug.Log("Building add " + TestMain.GetMap().GetTile(givenBuilding));
        TestMain.AddElement<Tile>(ref m_tile, TestMain.GetMap().GetTile(givenBuilding));
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
