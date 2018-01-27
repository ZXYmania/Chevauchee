﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Character m_character;
    public Character GetCharacter() { return m_character; }
    GameMode[] m_mode;
    public GameMode GetMode(ModeType givenModeType) { return m_mode[(int)givenModeType]; }
    ModeType currMode;
    public ModeType GetCurrentModeType() { return currMode; }
    public GameMode GetCurrentMode(){return m_mode[(int)currMode];}
    public void SetModeType(ModeType givenMode) { currMode = givenMode; }
    private Menu m_menu;
    //first tile is hover second is selected
	// Use this for initialization
	void Start ()
    {
       m_mode = GameMode.CreateModes(this);
       currMode = 0;
    }
    public void Initialise(Character givenCharacter)
    {
        m_character = givenCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        bool[] currMouseDown = new bool[] { Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1) };
        if (currMouseDown[0] || currMouseDown[1])
        {
            m_mode[(int)currMode].OnClick(currMouseDown);
        }
        else
        {
            m_mode[(int)currMode].OnHover();
        }
        m_mode[(int)currMode].UI();
    }

    public void AddBuilding(Tile givenTile, string buildingType)
    {
        Building.AddBuilding(givenTile, buildingType, m_character);
    }
}
