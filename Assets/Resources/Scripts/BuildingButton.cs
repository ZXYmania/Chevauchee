using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MenuItem
{
    string m_building;
    public static BuildingButton CreateBuildingButton(string givenBuilding, Color givenColour)
    {
        BuildingButton output = CreateMenuItem<BuildingButton>("Build"+givenBuilding, givenColour);
        output.Initialise(givenBuilding);
        return output;
    }
    protected void Initialise(string givenBuilding)
    {
        m_building = givenBuilding;
    }
    protected override void Select(bool selected, Player givenPlayer)
    {
        BuildMode temp = (BuildMode)givenPlayer.GetCurrentMode();
        temp.SetBuilding(m_building);
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
