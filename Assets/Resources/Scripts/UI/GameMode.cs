using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModeType
{
    observe = 0,
    build = 1,
    army = 2,
}

public abstract class GameMode 
{
    protected Player m_player;
    protected LayerMask m_layerMask;
    protected ClickAble[] selected;
    protected ClickAble hover;
    protected int clickLength;
    protected Menu[] m_menu;

    public static GameMode[] CreateModes(Player givenPlayer)
    {
        GameMode[] allMode = new GameMode[2];
        allMode[0] = new ObserveMode(givenPlayer);
        allMode[1] = new BuildMode(givenPlayer);
        return allMode;
    }

    public static bool ChangeStates(Player givenPlayer, ModeType newMode, ClickAble clickItem = null)
    {
        if (givenPlayer.GetCurrentMode().OnModeExit())
        {
            if(clickItem != null)
            {
                if (givenPlayer.GetMode(newMode).OnModeEnter(clickItem))
                {
                    givenPlayer.SetModeType(newMode);
                }
            }
            else
            {
                if (givenPlayer.GetMode(newMode).OnModeEnter())
                {
                    givenPlayer.SetModeType(newMode);
                }
            }

        }
        return true;
    }

    

    protected GameMode(Player givenPlayer)
    {
        m_player = givenPlayer;
        selected = new ClickAble[0];
    }
    public abstract void OnClick(bool[] givenClick);
    public abstract void OnHover();
    public abstract bool OnModeEnter();
    public abstract bool OnModeEnter(ClickAble givenSelected);
    public abstract bool OnModeExit();
    public abstract void UI();
    public GameObject FindObjectUnder()
    {
        Ray mousePoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D foundItem = Physics2D.GetRayIntersection(mousePoint, Mathf.Infinity, m_layerMask);
        if(foundItem.transform != null)
        {
            return foundItem.transform.gameObject;
        }
        return null;
    }
}

public interface ClickAble
{
    bool IsSelected();
    void Selected(bool isSelected);
}

public class ObserveMode : GameMode
{
    public ObserveMode(Player givenPlayer) : base(givenPlayer)
    {
        m_layerMask = -1;
        m_menu = new Menu[1];
        for(int i = 0; i < m_menu.Length; i++)
        {
            GameObject tempObject = new GameObject();
            tempObject.AddComponent<SpriteRenderer>();
            m_menu[i] = new ObserverMenu();
        }
        
    }
    public override void OnClick(bool[] givenClick)
    {
        if(givenClick[0])
        {
            GameObject cursorObj = FindObjectUnder();
            if (cursorObj != null)
            {
                ClickAble clickItem = cursorObj.GetComponent<ClickAble>();
                if (clickItem != null)
                {
                    if (clickItem.GetType() == typeof(Tile))
                    {
                        Tile clickTile = clickItem as Tile;
                        if (clickTile.GetDomain() != null)
                        {
                            if (m_player.GetCharacter().RulesDomain(clickTile.GetDomain()))
                            {
                                Building[] clickBuilding = clickTile.GetBuildings();
                                if (clickBuilding.Length > 0)
                                {
                                    ChangeStates(m_player, ModeType.build, clickItem);
                                }    
                            }
                        }
                        TestMain.GetCamera().transform.position = new Vector3(clickTile.GetX(), clickTile.GetY(),TestMain.GetCamera().transform.position.z);
                    }
                }
            }
        }
    }

    public override void OnHover()
    {
        GameObject cursorObj = FindObjectUnder();
        if (cursorObj != null)
        {
            ClickAble cursorItem = cursorObj.GetComponent<ClickAble>();
            if (cursorItem != null)
            {
                if (cursorItem.GetType() == typeof(Tile))
                {
                    if (hover != null)
                    {
                        hover.Selected(false);
                    }
                    hover = cursorItem;
                }
            }
        }
    }

    public override bool OnModeEnter()
    {
        return true;
    }

    public override bool OnModeEnter(ClickAble givenSelected)
    {
        throw new NotImplementedException();
    }

    public override bool OnModeExit()
    {
        hover = null;
        selected = null;
        return true;
    }

    public override void UI()
    {
        m_menu[0].UI();
    }
}

public class BuildMode : GameMode
{
    Tile[] currPath;
    public BuildMode(Player givenPlayer) : base(givenPlayer)
    {
        m_layerMask = -1;
        currPath = new Tile[0];
    }

    public override void OnClick(bool[] givenClick)
    {
        if (givenClick[0])
        {
            if (clickLength > 15)
            {
                GameObject cursorObj = FindObjectUnder();
                if (cursorObj != null)
                {
                    hover = null;
                    ClickAble cursorItem = cursorObj.GetComponent<ClickAble>();
                    if (cursorItem != null)
                    {
                        if (cursorItem.GetType() == typeof(Tile))
                        {
                            if (Building.CanBuildOnTile((Tile)cursorItem, "Road"))
                            {
                                if (selected.Length == 0)
                                {

                                    TestMain.AddElement<ClickAble>(ref selected, cursorItem);
                                    selected[0].Selected(true);
                                }
                                else
                                {
                                    TestMain.AddElement<ClickAble>(ref selected, (ClickAble[])currPath);
                                    if (givenClick[0])
                                    {
                                        for (int i = 0; i < selected.Length; i++)
                                        {
                                            m_player.AddBuilding((Tile)selected[i], "Road");
                                            selected[i].Selected(false);
                                        }
                                        selected = new ClickAble[0];
                                    }
                                }
                            }
                        }
                        clickLength = 0;
                    }
                }
            }
        }
        else if(givenClick[1])
        {
            if (selected.Length < 1)
            {
                ChangeStates(m_player, ModeType.observe);
            }
            else
            {
                ClearSelection();
            }
        }
    }

    public override void OnHover()
    {
        GameObject cursorObj = FindObjectUnder();
        if (cursorObj != null)
        {
            ClickAble cursorItem = cursorObj.GetComponent<ClickAble>();
            if (cursorItem != null)
            {
                if (cursorItem.GetType() == typeof(Tile))
                {

                    if (selected.Length > 0)
                    {
                        try
                        {
                            Tile[] tempPath = TestMain.GetMap().GetBestPath((Tile)selected[selected.Length - 1], (Tile)cursorItem, PathType.CompareTerrain);
                            if (currPath != null)
                            {
                                for (int i = 0; i < currPath.Length; i++)
                                {
                                    currPath[i].Selected(false);
                                }
                            }
                            currPath = tempPath;
                            for (int i = 0; i < currPath.Length; i++)
                            {
                                currPath[i].Selected(true);
                            }
                            for (int i = 0; i < selected.Length; i++)
                            {
                                selected[i].Selected(true);
                            }
                        }
                        catch (NoPathException e)
                        {
                            //hit ocean or imppassible
                        }
                    }
                    else
                    {
                        if (hover != null)
                        {
                            hover.Selected(false);
                        }
                        hover = cursorItem;
                        hover.Selected(true);
                    }
                }
            }
            clickLength++;
        }
        
    }

    public override bool OnModeEnter()
    {
        return true;
    }

    public override bool OnModeEnter(ClickAble currentClick)
    {
        TestMain.AddElement<ClickAble>(ref selected,currentClick);
        return OnModeEnter();
    }


    public override bool OnModeExit()
    {
        ClearSelection();
        if (hover != null)
        {
            hover.Selected(false);
        }
        hover = null;
        return true;
    }

    public bool ClearSelection()
    {
        for (int i = 0; i < selected.Length; i++)
        {
            selected[i].Selected(false);
        }
        selected = new ClickAble[0];
        for (int i = 0; i < currPath.Length; i++)
        {
            currPath[i].Selected(false);
        }
        currPath = new Tile[0];
        return true;
    }


    public override void UI()
    {
        //throw new NotImplementedException();
    }
}
