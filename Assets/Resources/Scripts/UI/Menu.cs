using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Menu 
{
    protected Canvas m_canvas;
    protected virtual void Initialise()
    {
       // m_canvas =  ;
    }

    public void UI()
    {

    }
}

public class InfoPane : MonoBehaviour
{

}



public class ObserverMenu : Menu
{
    Button[] m_button;
    public ObserverMenu() 
    {
        Initialise();
    }
    protected override void Initialise()
    {
        /*
        base.Initialise();
        m_button = new Button[1];
        m_button[0] = m_canvas.gameObject.AddComponent<Button>();
        Dictionary<string, PlayAnimation> currentAnimation = TextureController.GetAnimations("Buildmode", Color.black);
        string animationName = "BuildMode" + Color.black + 0;
        m_button[0].image.sprite = currentAnimation[animationName].GetSprite(0);
        m_button[0].transform.position = new Vector3(0, 0, 0);
        */
    }
}
