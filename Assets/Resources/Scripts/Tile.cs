using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Animated , ClickAble
{
    public enum Terrain
    {
        blank = 0,
        ocean = 1,
        forest = 2,
        smooth = 3,
        rough = 4,
        mountain = 5,
    }

    public static Tile CreateTile(int xPos, int yPos)
    {
        GameObject m_gameObject = new GameObject();
        m_gameObject.transform.localScale = new Vector3 (2, 2, 1);
        m_gameObject.layer = LayerMask.NameToLayer("Tile");
        Tile outPutTile = m_gameObject.AddComponent<Tile>();
        outPutTile.Initialise(xPos, yPos);
        BoxCollider2D m_collider = m_gameObject.AddComponent<BoxCollider2D>();
        m_collider.size = new Vector2(1 / m_gameObject.transform.localScale.x,
                                                        1 / m_gameObject.transform.localScale.y);
        m_collider.offset = new Vector2(0.5f / m_collider.transform.localScale.x, 0.5f / m_collider.transform.localScale.y);
        return outPutTile;
    }

    public static bool IsImpassible(Tile givenTile, PathType givenPathType)
    {
        switch (givenPathType ) 
        {
            case PathType.ImpassableTerrain:
             switch (givenTile.GetTerrain())
                {
                    case Terrain.ocean:
                    case Terrain.mountain:
                        return true;
                    default: return false;
                }
            case PathType.NavalRoute:
                switch (givenTile.GetTerrain())
                {
                    case Terrain.ocean: return false;
                    default: return true;
                }
            default: return false;
        }

    }
    public int[] GetPosition() { return new int[] { GetX(), (int) GetY() };}
    public int GetX() { return (int)GetTransformX();}
    public int GetY() { return (int)GetTransformY();}

    Building[] m_building;
    public Building[] GetBuildings() { return m_building; }
    string m_domain;
    public Terrain m_terrain;
    ModeType currentMode;
    public void Selected(bool givenSelected, Player givenPlayer) { m_animationLayer["Selected"].SetVisible(givenSelected); }
    public bool IsSelected() { return m_animationLayer["Selected"].GetVisible(); }
    public void Hover(bool givenHover, Player givenPlayer) { }

    protected void Initialise(int xPos, int yPos)
    {        
        m_building = new Building[0];
        m_terrain = Terrain.blank;
        SetTransform(xPos, yPos);
        base.Initialise();
        gameObject.layer = LayerMask.NameToLayer("Tile");
        gameObject.name = ToString();
        AddAnimationLayer("Terrain", "Terrain", new Color(0, 0, 0, 1), true);
        AddAnimationLayer("Selected", "Selected", new Color(0, 0, 0, 1), true);
        m_animationLayer["Selected"].SetAnimationPosition(0, 0, -2);
        int probability = UnityEngine.Random.Range(0, 10);
        if(probability < 4)
        {
            m_terrain = Terrain.smooth;
        }
        else if(probability < 7)
        {
            m_terrain = Terrain.forest;
        }
        else if(probability < 9)
        {
            m_terrain = Terrain.rough;
        }
        else
        {
            m_terrain = Terrain.mountain;
        }
        ChangeAnimation("Terrain", (int)m_terrain);
        m_animationLayer["Selected"].SetVisible(false);
    }

    public void Update()
    {
        Player currentPlayer = TestMain.GetPlayer();
        ModeType newMode = currentPlayer.GetCurrentModeType();
        if (currentMode != newMode)
        {
            switch (currentPlayer.GetCurrentModeType())
            {
                case ModeType.army:
                    break;
                case ModeType.build:
                    if (m_building.Length > 0)
                    {
                        m_animationLayer["Overlay"].SetVisible(true);
                        ChangeSpriteMap("Overlay", m_building[0].ToString());

                    }
                    else if (m_domain != null)
                    {
                        if (currentPlayer.GetCharacter().RulesDomain(m_domain))
                        {
                            m_animationLayer["Overlay"].SetVisible(false);
                        }
                        else
                        {
                            m_animationLayer["Overlay"].SetVisible(true);
                            ChangeSpriteMap("Overlay", m_domain);
                        }
                        break;
                    }
                    break;
                case ModeType.observe:
                    if (m_building.Length > 0){ m_animationLayer["Overlay"].SetVisible(true); ChangeSpriteMap("Overlay", m_building[0].ToString());
                    }
                    else if (m_domain != null){
                        m_animationLayer["Overlay"].SetVisible(true);ChangeSpriteMap("Overlay", m_domain);
                    }
                    break;
            }
            currentMode = newMode;
        }
        Animate();
    }

    public Terrain GetTerrain() { return m_terrain; }
    public float GetObstacle(PathType givenPathtype)
    {
        switch (givenPathtype)
        {
            case PathType.CompareTerrain:
            if (m_building.Length > 0)
            {
                return 0.8f;
            }
            switch (m_terrain)
            {
                case Terrain.ocean: return -1;
                case Terrain.mountain: return -1;
                case Terrain.smooth: return 1.2f;
                case Terrain.rough: return 1.6f;
                case Terrain.forest: return 1.3f;
                default: return 1;
            }
            case PathType.NavalRoute:
                switch (m_terrain)
                {
                    case Terrain.ocean: return 1;
                    default: return -1;
                }
            case PathType.RoadOnly :
                for (int i =0; i < m_building.Length; i++)
                {
                    if (m_building[i].GetType() == typeof(Road))
                    {
                        return 0.2f;
                    }
                }
                return -1;
            case PathType.ImpassableTerrain:
                switch(m_terrain)
                {
                    case Terrain.mountain: return -1;
                    case Terrain.ocean: return -1;
                    default: return 1;
                }
            default: return 1;
        }
    }
    public void SetDomain(string givenDomain)
    {
        if(DomainDictionary.Contains(givenDomain) && m_terrain != Terrain.ocean)
        {
            m_domain = givenDomain;
            AddAnimationLayer("Overlay", givenDomain, new Color(0, 0, 0, 1), true);
            m_animationLayer["Overlay"].SetAnimationPosition(0, 0, -1);


        }
        else
        {
            Debug.Log("Domain "+ givenDomain +" not set ");
        }
    }
    public string GetDomain() { return m_domain; }
    public void SetTerrain(Terrain givenTerrain)
    {
        if(m_terrain == Terrain.blank || givenTerrain == Terrain.ocean)
        {
            m_terrain = givenTerrain;
            ChangeSpriteMap("Terrain","Terrain",(int)m_terrain);
        }
    }


    public bool AddBuilding(Building givenBuilding)
    {
        bool haveBuilding = false;
        
            if (m_building.Length > 0 )
            {
                for (int i = 0; i < m_building.Length; i++)
                {
                    if (m_building[i].GetName() == givenBuilding.GetName())
                    {
                        haveBuilding = true;
                        i = m_building.Length;
                    }
                }
            }
            if (!haveBuilding)
            {
                TestMain.AddElement<Building>(ref m_building, givenBuilding);
                AddAnimationLayer("Overlay", givenBuilding.ToString(), new Color(0, 0, 0, 1), true);
                m_animationLayer["Overlay"].SetAnimationPosition(0, 0, -1);
                ChangeSpriteMap("Overlay", m_building[0].ToString());
                m_animationLayer["Overlay"].SetVisible(true);
                return true;
            }
        return false;
    }
    public override string ToString()
    {
        return "{X : "+GetX()+", Y: "+GetY()+"}" ;
    }

    public static bool Match(Tile firstGiven, Tile secondGiven)
    {
        if(firstGiven != null && secondGiven != null)
        {
            return firstGiven.ToString() == secondGiven.ToString();
        }
        return false;
    }
}
