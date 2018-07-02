using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Character m_character;
    public Character GetCharacter() { return m_character; }

    GameMode[] m_mode;
    public GameMode GetMode(ModeType givenModeType) { return m_mode[(int)givenModeType]; }
    public Position bottomLeft;
    public Position topRight;
    bool mapLoaded;
    ModeType currMode;
    public ModeType GetCurrentModeType() { return currMode; }
    public GameMode GetCurrentMode(){return m_mode[(int)currMode];}
    public void SetModeType(ModeType givenMode) { currMode = givenMode; }
    private MenuController m_menu;
    //first tile is hover second is selected
	// Use this for initialization
	void Start ()
    {

    }
    public void Initialise(Character givenCharacter)
    {
        m_character = givenCharacter;
        m_mode = GameMode.CreateModes(this);
        m_menu = new MenuController();
        currMode = 0;
        bottomLeft = new Position((int)transform.position.x-1, (int)transform.position.y-1);
        topRight = new Position((int)transform.position.x, (int)transform.position.y);
        TestMain.GetMap().CreateMapView(ref bottomLeft, ref topRight);
        mapLoaded = false;
    }
    public bool ChangeStates( ModeType newMode, ClickAble clickItem = null)
    {
        if (GetCurrentMode().OnModeExit())
        {
            if (clickItem != null)
            {
                if (GetMode(newMode).OnModeEnter(clickItem))
                {
                    m_menu.ChangeMenu((ModeType)currMode, newMode);
                    SetModeType(newMode);
                }
            }
            else
            {
                if (GetMode(newMode).OnModeEnter())
                {
                    m_menu.ChangeMenu((ModeType)currMode, newMode);
                    SetModeType(newMode);
                }
            }

        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                if (TestMain.GetCamera().orthographicSize > 25)
                {
                    TestMain.GetCamera().orthographicSize -= 5;
                }
            }
            else
            {
                if (TestMain.GetCamera().orthographicSize < 200)
                {
                    TestMain.GetCamera().orthographicSize += 5;
                }
            }
        }
        bool[] currMouseDown = new bool[] { Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1) };
        if (currMouseDown[0] || currMouseDown[1])
        {
            m_mode[(int)currMode].OnClick(currMouseDown);
        }
        else
        {
            m_mode[(int)currMode].OnHover();
        }
        if (!mapLoaded)
        {
            mapLoaded = TestMain.GetMap().ExtendBound(ref bottomLeft, ref topRight);
        }
    }

    public void AddBuilding(Tile givenTile, string buildingType)
    {
        Building.AddBuilding(givenTile, buildingType, m_character);
    }
}
