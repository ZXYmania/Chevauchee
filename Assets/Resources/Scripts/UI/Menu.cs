using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Menu 
{
    protected MenuItem[] m_item;
    protected virtual void Initialise()
    {
        Canvas canvas = TestMain.GetCanvas();
        GameObject canvasObj = new GameObject();
      //  canvas = canvasObj.AddComponent<Canvas>();
        canvas.transform.position = new Vector3(0, 0, -3);
        m_item = new MenuItem[0];
    }
    public void Visible(bool givenVisible)
    {
        for(int i = 0; i < m_item.Length;i++ )
        {
            m_item[i].Visible(givenVisible);
        }
    }
    public void UI()
    {

    }
}

public class OverLayMenu : Menu
{
    public OverLayMenu()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();
        MenuItem observerModebuttonItem = MenuButton.CreateMenuButton("ObserverMode", Color.black, ModeType.observe);
        observerModebuttonItem.transform.SetParent(TestMain.GetCanvas().transform);
        TestMain.AddElement<MenuItem>(ref m_item, observerModebuttonItem);
        m_item[(int)ModeType.observe].transform.localPosition = new Vector3(-25, -15, 0);
        m_item[(int)ModeType.observe].Visible(false);
        MenuItem buildModeButton = MenuButton.CreateMenuButton("BuildMode", Color.black, ModeType.build);
        buildModeButton.transform.SetParent(TestMain.GetCanvas().transform);
        TestMain.AddElement<MenuItem>(ref m_item, buildModeButton);
        m_item[(int)ModeType.build].transform.localPosition = new Vector3(-25, -15, 0);
        m_item[(int)ModeType.build].Visible(true);

    }
    public void ChangeMode(ModeType oldMode, ModeType newMode)
    {
        m_item[(int)oldMode].Visible(true);
        m_item[(int)newMode].Visible(false);
    }
}



public class ObserverMenu : Menu
{
    public ObserverMenu() 
    {
        Initialise();
    }
    protected override void Initialise()
    {
        base.Initialise();
    }
}

public class BuildingMenu : Menu
{
    public BuildingMenu()
    {
        Initialise();
        BuildingButton observerModebuttonItem = BuildingButton.CreateBuildingButton("Farm", Color.black);
        observerModebuttonItem.transform.SetParent(TestMain.GetCanvas().transform);
        TestMain.AddElement<MenuItem>(ref m_item, observerModebuttonItem);
        m_item[0].transform.localPosition = new Vector3(20, 10, 0);
        m_item[0].Visible(true);
        m_item[0].SetSize(10,10);
    }
    protected override void Initialise()
    {
        base.Initialise();

    }
}
