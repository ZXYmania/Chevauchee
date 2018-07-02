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

    public static GameMode[] CreateModes(Player givenPlayer)
    {
        GameMode[] allMode = new GameMode[3];
        allMode[(int)ModeType.observe] = new ObserveMode(givenPlayer);
        allMode[(int)ModeType.build] = new BuildMode(givenPlayer);
       // allMode[(int)ModeType.army]
        return allMode;
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
    void Selected(bool isSelected, Player givenPlayer);
    void Hover(bool isHover, Player givenPlayer);
}

public class ObserveMode : GameMode
{
    public ObserveMode(Player givenPlayer) : base(givenPlayer)
    {
        m_layerMask = (1 << 8);
        m_layerMask |= (1 << 2);
        m_layerMask = ~m_layerMask;
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
                    if (cursorObj.layer == LayerMask.NameToLayer("Tile"))
                    {
                        Tile clickTile = clickItem as Tile;
                        if (clickTile.GetDomain() != null)
                        {
                            if (m_player.GetCharacter().RulesDomain(clickTile.GetDomain()))
                            {
                                Building[] clickBuilding = clickTile.GetBuildings();
                                if (clickBuilding.Length > 0)
                                {
                                    m_player.ChangeStates(ModeType.build, clickItem);
                                }    
                            }
                        }
                        TestMain.MoveCamera(clickTile.GetX(), clickTile.GetY());
                    }
                    if(cursorObj.layer == LayerMask.NameToLayer("UI"))
                    {
                        selected = new ClickAble[] { clickItem };
                        selected[0].Selected(true, m_player);
                    }
                    if(selected.Length>0)
                    {
                        for(int i =0; i < selected.Length;i++)
                        {
                            selected[i].Selected(false, m_player);
                        }
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
                if (cursorObj.layer == LayerMask.NameToLayer("Tile"))
                {
                    if (hover != null)
                    {
                        hover.Hover(false, m_player);
                    }
                    hover = cursorItem;
                    hover.Hover(true, m_player);
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
        hover.Selected(false,m_player);
        hover = null;
        selected = new ClickAble[0];
        return true;
    }
}

public class BuildMode : GameMode
{
    Tile[] currPath;
    string currbuilding;
    public BuildMode(Player givenPlayer) : base(givenPlayer)
    {
        m_layerMask = (1 << 8);
        m_layerMask |= (1 << 2);
        m_layerMask = ~m_layerMask;
        currPath = new Tile[0];
        currbuilding = "Road";
    }

    public void SetBuilding(string givenBuilding)
    {
        currbuilding = givenBuilding;
    }

    public override void OnClick(bool[] givenClick)
    {
        if (hover != null)
        {
            hover.Selected(false, m_player);
        }
        hover = null;
        if (givenClick[0])
        {
            if (clickLength > 15)
            {
                GameObject cursorObj = FindObjectUnder();
                if (cursorObj != null)
                {
                    hover = null;
                    ClickAble clickItem = cursorObj.GetComponent<ClickAble>();
                    if (clickItem != null)
                    {
                        if (cursorObj.layer == LayerMask.NameToLayer("Tile"))
                        {
                            if (Building.CanBuildOnTile((Tile)clickItem, currbuilding, m_player))
                            {
                                if (currbuilding != "Road")
                                {
                                    m_player.AddBuilding((Tile)clickItem, currbuilding);
                                    selected = new ClickAble[0];
                                }
                                else
                                {
                                    if (selected.Length == 0)
                                    {
                                        TestMain.AddElement<ClickAble>(ref selected, clickItem);
                                        selected[0].Selected(true, m_player);
                                    }
                                    else
                                    {
                                        TestMain.AddElement<ClickAble>(ref selected, (ClickAble[])currPath);
                                        if (givenClick[0])
                                        {
                                            for (int i = 0; i < selected.Length; i++)
                                            {
                                                m_player.AddBuilding((Tile)selected[i], currbuilding);
                                                selected[i].Selected(false, m_player);
                                            }
                                            selected = new ClickAble[0];
                                        }
                                    }
                                }
                            }
                        }
                        if (cursorObj.layer == LayerMask.NameToLayer("UI"))
                        {
                            selected = new ClickAble[] { clickItem };
                            selected[0].Selected(true, m_player);
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
                m_player.ChangeStates(ModeType.observe);
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
                if (cursorObj.layer == LayerMask.NameToLayer("Tile"))
                {
                    if (Building.CanBuildOnTile((Tile)cursorItem, currbuilding, m_player))
                    {
                        if (currbuilding != "Road")
                        {
                            if(hover!=null)
                            {
                                hover.Hover(false, m_player);
                            }
                            hover = cursorItem;
                            cursorItem.Hover(true, m_player);
                        }
                        else
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
                                            currPath[i].Selected(false, m_player);
                                        }
                                    }
                                    currPath = tempPath;
                                    for (int i = 0; i < currPath.Length; i++)
                                    {
                                        currPath[i].Hover(true, m_player);
                                    }
                                    for (int i = 0; i < selected.Length; i++)
                                    {
                                        selected[i].Selected(true, m_player);
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
                                    hover.Selected(false, m_player);
                                }
                                if (Building.CanBuildOnTile((Tile)cursorItem, currbuilding, m_player))
                                {
                                    hover = cursorItem;
                                    hover.Selected(true, m_player);
                                }
                            }
                        }
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
        return true;
    }

    public bool ClearSelection()
    {
        for (int i = 0; i < selected.Length; i++)
        {
            selected[i].Selected(false, m_player);
        }
        selected = new ClickAble[0];
        for (int i = 0; i < currPath.Length; i++)
        {
            currPath[i].Selected(false, m_player);
        }
        if (hover != null)
        {
            hover.Selected(false, m_player);
        }
        hover = null;
        currPath = new Tile[0];
        return true;
    }
}
