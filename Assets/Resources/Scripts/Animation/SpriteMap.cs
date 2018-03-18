using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteMap 
{

	List<PlayAnimation> m_animationList;
    private string m_base;
    public string GetBase()
    {
        if(m_base == null)
        {
            return m_name;
        }
        return m_base;
    }
	public string m_name;
	Color m_borderColour;
    int m_baseAnimation;
    public int GetBaseAnimationAsInt() { return m_baseAnimation; }
    public void SetBaseAnimation(int givenAnimation) { m_baseAnimation = givenAnimation; }
	public void SetName(string givenName){m_name = givenName;}
	public string GetName(){return m_name;}
	public Color GetColour(){return m_borderColour;}
	public List<PlayAnimation> GetAnimations()
	{
		return m_animationList;
	}
	public PlayAnimation GetAnimation(int givenAnimation=0)
    {
        return m_animationList[givenAnimation];
    }
	public void AddAnimation(PlayAnimation givenAnimation)
	{
			PlayAnimation[] temp = new PlayAnimation[m_animationList.Count + 1];
            m_animationList.Add(givenAnimation);
	}
	
	public void Initialise(string givenName, Color givenColour)
	{
		m_name = givenName;
        int indexChar = m_name.IndexOf("_");
        if (indexChar !=-1)
        {
            m_base = m_name.Remove(indexChar);
        }
		m_borderColour = givenColour;
        m_animationList = new List<PlayAnimation>();
        m_baseAnimation = 0;
	}
}
