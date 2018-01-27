using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class City :  Building 
//{
    
//}


public class Farm : Industrial
{
    public Farm()
    {
        m_population = 200;
        m_efficiency = 2.4f;
        m_production = new ResourceList();
        m_production.Add(resource.wheat, (int)Mathf.Floor(m_population * m_efficiency));
        m_maintenance = new ResourceList();
        m_maintenance.Add(resource.wheat, (int)Mathf.Floor(m_population * 1.2f));
        m_maintenance.Add(resource.wood, (int)Mathf.Floor(m_population * 0.4f));
        m_name = "Farm";
    }
    public override int GetTax() { return m_population;}

    public override string ToString()
    {
        return "Farm";
    }
}

public class LumberMill : Industrial
{
    public LumberMill()
    {
        m_population = 180;
        m_efficiency = 0.8f;
        m_production.Add(resource.wood, (int)Mathf.Floor(m_population * m_efficiency));
        m_maintenance = new ResourceList();
        m_maintenance.Add(resource.wheat, (int)Mathf.Floor(m_population * 1.2f));
        m_maintenance.Add(resource.iron, (int)Mathf.Floor(m_population * 0.2f));
        m_name = "LumberMill";
    }
    public override int GetTax()
    {
       return m_population;
    }
    public override string ToString()
    {
        return "LumberMill";
    }
}
public class City : Millitirised
{
    public City() : base()
    {
        m_population = 180;
        m_maintenance.Add(resource.wheat, (int)Mathf.Floor(m_population * 1.3f));
        m_name = "City";
    }
    public override int GetTax()
    {
        return 0;
    }
    public override string ToString()
    {
        return "City";
    }
}

public class Road : Building
{
    public override int GetTax(){ return 0;}
    public Road()
    {
        m_maintenance.Add(resource.gold, (int)Mathf.Floor(10));
        m_name = "Road";
    }
    public override string ToString()
    {
        return "Road";
    }
}
