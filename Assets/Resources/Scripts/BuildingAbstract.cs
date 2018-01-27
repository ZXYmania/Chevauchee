using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : ClickAble
{
    public static bool AddBuilding(Tile givenTile, string buildingType, Character builder)
    {
        Building newBuilding;
        switch (buildingType)
        {
            case "Farm":
                newBuilding = new Farm();
                break;
            case "LumberMill":
                newBuilding = new LumberMill();
                break;
            case "City":
                newBuilding = new City();
                break;
            case "Road":
                newBuilding = new Road();
                break;
            default:
                newBuilding = null;
                throw new KeyNotFoundException("The building type: " + buildingType + " does not exist and an attempt was made to create it");
        }
        
        if (newBuilding != null && builder.RulesDomain(givenTile.GetDomain()))
        {
            givenTile.AddBuilding(newBuilding);
            DomainDictionary.GetDomain(givenTile.GetDomain()).AddBuilding(givenTile.GetPosition());
            return true;
        }
        else
        {
            Debug.Log("You can't build " + newBuilding + " at " + givenTile.GetDomain());
            return false;
        }
    }

    public static bool CanBuildOnTile(Tile givenTile, string buildingType)
    {
        switch(givenTile.GetTerrain())
        {
            case Tile.Terrain.forest: return true;
            case Tile.Terrain.smooth: return true;
            case Tile.Terrain.rough: return true;
            case Tile.Terrain.mountain: break;
            case Tile.Terrain.ocean:break;

        }
        return false;
    }

    protected Building()
    {
        m_maintenance = new ResourceList();
    }
    bool selected;
    protected string m_name;
    public string GetName() { return m_name; }
    protected int m_population;
    protected Culture m_culture;
    protected Religion[] m_religion;
    protected ResourceList m_maintenance;
    public abstract int GetTax();
    public virtual ResourceList GetMaintenance() { return m_maintenance; }
	public void Selected(bool givenSelected) { selected = givenSelected; }
    public bool IsSelected() { return selected; }
}

public abstract class Industrial : Building 
{
    protected ResourceList m_production;
    protected float m_efficiency;
    public Industrial() : base()
    {
        m_production = new ResourceList();
    }
    public virtual ResourceList ProduceResource() { return m_production; }

}

public abstract class Millitirised : Building
{
    public Millitirised() : base()
    {

    }
    private Garrison m_garrison;
}
