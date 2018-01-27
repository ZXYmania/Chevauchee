using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteMap 
{

	Dictionary<string,PlayAnimation> m_animationList;
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
	public void SetName(string givenName){m_name = givenName;}
	public string GetName(){return m_name;}
	public Color GetColour(){return m_borderColour;}
	public Dictionary<string, PlayAnimation> GetAnimations()
	{
		return m_animationList;
	}
	
	public void AddAnimation(PlayAnimation givenAnimation)
	{
			PlayAnimation[] temp = new PlayAnimation[m_animationList.Count + 1];
            string animationName = m_name+ m_borderColour + m_animationList.Count;
            m_animationList.Add(animationName, givenAnimation);
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
        m_animationList = new Dictionary<string, PlayAnimation>();
	}
}
