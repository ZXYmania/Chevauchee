using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MenuItem
{
    ModeType m_mode;
    public static MenuButton CreateMenuButton(string givenTexture, Color givenColor, ModeType givenModeType)
    {
        MenuButton output = CreateMenuItem<MenuButton>(givenTexture, givenColor);
        output.Initialise(givenModeType);
        return output;
    }
    protected void Initialise(ModeType givenModeType)
    {
        m_mode = givenModeType;
        SetSize(10,10);
    }
    protected override void Select(bool selected, Player givenPlayer)
    {
        if(selected)
        {
            givenPlayer.ChangeStates(m_mode);
        }
    }
}
