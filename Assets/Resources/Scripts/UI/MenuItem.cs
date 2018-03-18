using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : Animated, ClickAble
{
    protected bool selected;
    protected bool hover;
    protected static GameObject CreateObject()
    {
        GameObject m_gameObject = new GameObject();
        m_gameObject.layer = LayerMask.NameToLayer("UI");
        BoxCollider2D m_collider = m_gameObject.AddComponent<BoxCollider2D>();
        m_collider.offset = new Vector2(0.5f, 0.5f);
        return m_gameObject;
    }
    public static T CreateMenuItem<T>() where T : MenuItem 
    {
        GameObject m_gameObject = CreateObject();
        m_gameObject.name = typeof(T).ToString(); 
        T outPutItem = m_gameObject.AddComponent<T>();
        outPutItem.Initialise();
        return outPutItem;
    }
    public static T CreateMenuItem<T>(string givenTexture, Color givenColor) where T : MenuItem
    {
        GameObject m_gameObject = CreateObject();
        m_gameObject.name = givenTexture+" menuItem";
        T outPutItem = m_gameObject.AddComponent<T>();
        outPutItem.Initialise();
        outPutItem.AddAnimationLayer("UI",givenTexture,givenColor,true);
        outPutItem.SetSize(1,1);
        return outPutItem;
    }

    public void SetSize(int givenWidth, int givenHeight)
    {
        Vector2 animationScale = m_animationLayer["UI"].GetAnimationSize();
        if ((animationScale.x / animationScale.y) == (float)(givenWidth / givenHeight))
        {
            gameObject.transform.localScale = new Vector3(givenWidth,givenHeight, 1);
        }
        else
        {
            throw new WrongFrameSizeException(m_animationLayer["UI"].GetAnimationName()+"with a width and height of "+animationScale.x+", "+animationScale.y + "is trying to be incorrectly scaled to "+givenWidth+" / "+givenHeight);
        }
    }
    bool ClickAble.IsSelected()
    {
        return selected;
    }

    void ClickAble.Selected(bool isSelected, Player givenPlayer)
    {
        selected = isSelected;
        Select(selected,givenPlayer);
    }
    void ClickAble.Hover(bool givenHover, Player givenPlayer)
    {
        Hover(givenHover, givenPlayer);
    }
    protected virtual void Select(bool selected, Player givenPlayer) { }

    protected virtual void Hover(bool givenHover, Player givenPlayer)
    {
        if (givenHover != hover)
        {
            if(givenHover)
            {
                SetSize((int)transform.localScale.x-1, (int)transform.localScale.y-1);
            }
            else
            {
                SetSize((int)transform.localScale.x + 1, (int)transform.localScale.y + 1);
            }
        }
    }

    public void Visible(bool givenVisible)
    {
        m_animationLayer["UI"].SetVisible(givenVisible);
        if (givenVisible)
        {
            gameObject.layer = LayerMask.NameToLayer("UI");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore");
        }
    }

    protected override void Initialise()
    {
        base.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }
}
