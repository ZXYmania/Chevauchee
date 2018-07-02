using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController 
{
    protected OverLayMenu m_overlay;
    protected Menu[] m_modeMenu;
    public MenuController()
    {
        m_modeMenu = new Menu[3];
        for (int i = 0; i < m_modeMenu.Length; i++)
        {
            GameObject tempObject = new GameObject();
            tempObject.AddComponent<SpriteRenderer>();
        }
        m_overlay = new OverLayMenu();
        m_modeMenu[(int)ModeType.observe] = new ObserverMenu();
        m_modeMenu[(int)ModeType.observe].Visible(true);
        m_modeMenu[(int)ModeType.build] = new BuildingMenu();
        m_modeMenu[(int)ModeType.build].Visible(false);

    }
    public bool ChangeMenu(ModeType oldMode, ModeType newMode)
    {
        m_modeMenu[(int)oldMode].Visible(false);
        m_modeMenu[(int)newMode].Visible(true);
        m_overlay.ChangeMode(oldMode, newMode);
        return true;
    }
}
