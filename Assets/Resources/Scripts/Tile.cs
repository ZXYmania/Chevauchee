using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public struct Position
{
    public int x;
    public int y;
    public Position(int givenX, int givenY)
    {
        x = givenX;
        y = givenY;
    }
}

public class Tile : ClickAble
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
    public static bool IsImpassible(Tile givenTile, PathType givenPathType)
    {
        switch (givenPathType)
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
    public int x { get; protected set; }
    public int y { get; protected set; }
    public Guid domain { get; protected set; }
    public Tile.Terrain terrain { get; protected set; }
    public Position GetPosition() { return new Position(x, y);}
    public int GetX() { return x;}
    public int GetY() { return y;}
    Building[] m_building;
    public Building[] GetBuildings() { return m_building; }
    ModeType currentMode;
    TileAnimation m_animation;
    public void Selected(bool givenSelected, Player givenPlayer) { m_animation.Select(givenSelected);}
    public bool IsSelected() { return m_animation.IsSelected(); }
    public void Hover(bool givenHover, Player givenPlayer) { m_animation.Select(givenHover);}

    public Tile()
    {

    }

    public Tile(int givenX, int givenY, Terrain givenTerrain, Domain givenDomain)
    {
        Initialise(givenX, givenY, givenTerrain, givenDomain);
    }
    public Tile(Position givenPos, Terrain givenTerrain, Domain givenDomain)
    {
        Initialise(givenPos.x, givenPos.y, givenTerrain, givenDomain);
    }

    protected void Initialise(int givenX, int givenY, Terrain givenTerrain, Domain givenDomain)
    {
        x = givenX;
        y = givenY;
        terrain = givenTerrain;
        domain = givenDomain.id;

    }
    public void LoadTile()
    {
        m_animation = TileAnimation.CreateAnimation(x,y,terrain,domain);
        m_animation.gameObject.layer = LayerMask.NameToLayer("Tile");
        m_animation.gameObject.name = ToString();
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
                        m_animation.SetBuilding(m_building[0]);
                        m_animation.ShowBuilding(true);
                    }
                    else if (domain != null)
                    {
                        if (currentPlayer.GetCharacter().RulesDomain(GetDomain()))
                        {
                            m_animation.ShowBuilding(false);
                        }
                        else
                        {
                            m_animation.ShowBuilding(true);
                            throw new NotImplementedException();
                            //ChangeSpriteMap("Overlay", m_domain);
                        }
                        break;
                    }
                    break;
                case ModeType.observe:
                    if (m_building.Length > 0){ m_animation.ShowBuilding(true);}
                    else if (domain != null)
                    {
                        throw new NotImplementedException();
                        //m_animationLayer["Overlay"].SetVisible(true);ChangeSpriteMap("Overlay", m_domain);
                    }
                    break;
            }
            currentMode = newMode;
        }
    }

    public Terrain GetTerrain() { return terrain; }
    public float GetObstacle(PathType givenPathtype)
    {
        switch (givenPathtype)
        {
            case PathType.CompareTerrain:
            if (m_building.Length > 0)
            {
                return 0.8f;
            }
            switch (terrain)
            {
                case Terrain.ocean: return -1;
                case Terrain.mountain: return -1;
                case Terrain.smooth: return 1.2f;
                case Terrain.rough: return 1.6f;
                case Terrain.forest: return 1.3f;
                default: return 1;
            }
            case PathType.NavalRoute:
                switch (terrain)
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
                switch(terrain)
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
        throw new NotImplementedException();
        /*if (DomainDictionary.Contains(givenDomain) && m_terrain != Terrain.ocean)
        {
            m_domain = givenDomain;
            AddAnimationLayer("Overlay", givenDomain, new Color(0, 0, 0, 1), true);
            m_animationLayer["Overlay"].SetAnimationPosition(0, 0, -1);


        }
        else
        {
            Debug.Log("Domain "+ givenDomain +" not set ");
        }*/
    }
    public Guid GetDomain() { return domain; }
    public void SetTerrain(Terrain givenTerrain)
    {
        if(terrain == Terrain.blank)
        {
            terrain = givenTerrain;
            m_animation.SetTerrain(terrain);
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
                m_animation.SetBuilding(givenBuilding);
                return true;
            }
        return false;
    }
    public string ToStringT()
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


    private class TileAnimation : Animated
    {
        public static TileAnimation CreateAnimation(int givenX, int givenY, Terrain givenTerrain, Guid givenDomain)
        {
            TileAnimation outPut = CreateAnimation();
            outPut.SetTransform(new Vector2(givenX, givenY));
            outPut.SetTerrain(givenTerrain);
            return outPut;
        }
        public static TileAnimation CreateAnimation()
        {
            GameObject m_gameObject = new GameObject();
            m_gameObject.transform.localScale = new Vector3(2, 2, 1);
            m_gameObject.layer = LayerMask.NameToLayer("Tile");
            TileAnimation outPutTile = m_gameObject.AddComponent<TileAnimation>();
            BoxCollider2D m_collider = m_gameObject.AddComponent<BoxCollider2D>();
            m_collider.size = new Vector2(1 / m_gameObject.transform.localScale.x, 1 / m_gameObject.transform.localScale.y);
            m_collider.offset = new Vector2(0.5f / m_collider.transform.localScale.x, 0.5f / m_collider.transform.localScale.y);
            outPutTile.Initialise();
            return outPutTile;
        }

        protected override void Initialise()
        {
            base.Initialise();
            AddAnimationLayer("Terrain", "Terrain", new Color(0, 0, 0, 1), true);
            AddAnimationLayer("Selected", "Selected", new Color(0, 0, 0, 1), true);
            m_animationLayer["Selected"].SetAnimationPosition(0, 0, -2);
            ChangeAnimation("Terrain", (int)Terrain.blank);
            m_animationLayer["Selected"].SetVisible(false);
        }
        void Update()
        {
            Animate();
        }
        public void SetTerrain(Terrain givenTerrain)
        {
            ChangeAnimation("Terrain", (int)givenTerrain);
        }
        public void ShowTerrain(bool visible)
        {
            m_animationLayer["Terrain"].SetVisible(visible);
        }
        public bool IsSelected()
        {
            return m_animationLayer["Selected"].GetVisible();
        }
        public void Select(bool givenSelected)
        {
            m_animationLayer["Selected"].SetVisible(givenSelected);
        }
        public void SetBuilding(Building givenBuilding)
        {
            if (!m_animationLayer.ContainsKey("Building"))
            {
                AddAnimationLayer("Building", givenBuilding.GetName(), new Color(0, 0, 0, 1), true);
                m_animationLayer["Building"].SetAnimationPosition(0, 0, -1);
            }
            ChangeSpriteMap("Building", givenBuilding.ToString());
        }
        public void ShowBuilding(bool visible)
        {
            m_animationLayer["Building"].SetVisible(visible);
        }
    }
}
