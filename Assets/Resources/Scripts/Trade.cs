using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Trade : object
{
    static TradeNetwork[] m_network;
    static Dictionary<string, int> m_networkDictionary;
    public static TradeNetwork GetNetwork(string givenNetworkName) { return m_network[m_networkDictionary[givenNetworkName]]; }
    public static void Initialise()
    {
        m_network = new TradeNetwork[1] { new TradeNetwork() };
        m_network[0].AddKingdom(KingdomDictionary.GetKingdom("Rome"));
    }
    public static void StartTrade()
    {
        for(int i = 0; i < m_network.Length; i++)
        {
            m_network[i].GetNetworkResource();
        }
        //Determine node trade
        //Do trade agreements
        //Do node trade
        //Send exports down stream
            //pay out exports
        //receive imports
            //Tell other node to pay out imports
    }
    public static void StartTradeKingdom(string givenKingdom)
    {
        m_network[0].DoDomesticTrade(KingdomDictionary.GetKingdom(givenKingdom));
    }
}

public class TradeNetwork 
{
    private ResourceList networkResource;
    private Kingdom[] m_kingdom;
    public void AddKingdom(Kingdom givenKingdom){ TestMain.AddElement<Kingdom>(ref m_kingdom, givenKingdom); }
    public void AddKingdom(Kingdom[] givenKingdom) { TestMain.AddElement<Kingdom>(ref m_kingdom, givenKingdom); }
    public TradeNetwork()
    {
        m_kingdom = new Kingdom[0];
        networkResource = new ResourceList();
    }
    public void GetNetworkResource()
    {
        //for each kingdom
        networkResource = new ResourceList();
        //int i = 0;
        // foreach (KeyValuePair<string, Kingdom> currKingdom in KingdomDictionary.GetDictionary())
        for(int i = 0; i <  m_kingdom.Length; i++)
        {
            networkResource.Add(m_kingdom[i].SetImportsExports());
            Debug.Log(m_kingdom[i].GetName() + " exports:" + m_kingdom[i].GetExports().ToString() + "imports: " + m_kingdom[i].GetImports().ToString());
        }
        Debug.Log(networkResource);
    }
    public void DoNetworkTrade()
    {
        for(int i = 0; i < ResourceList.AMOUNT_OF_RESOURCES; i++)
        {
            //If there is a surplis
            if(networkResource.GetAmount(ResourceList.ConvertInt(i)) > 0)
            {
                foreach(KeyValuePair<string, Kingdom> currKingdom in KingdomDictionary.GetDictionary())
                {
                    //Everyone buys all their resources
                    
                }

            }
            //if there is a deficit
            else if( networkResource.GetAmount(ResourceList.ConvertInt(i)) > 0)
            {
                foreach (KeyValuePair<string, Kingdom> currKingdom in KingdomDictionary.GetDictionary())
                {
                    //Everyone sells all their resources
                }
            }
        }
    }
    public ResourceList[] DoDomesticTrade(Kingdom givenKingdom)
    {
        Domain[] temp = givenKingdom.GetDomain();
        Domain[] realmDomain = new Domain [temp.Length];
        for(int i= 0; i < temp.Length; i++)
        {
            throw new NotImplementedException();
            //realmDomain[i] = DomainDictionary.GetDomain(temp[i].GetId());
        }
        for (int i = 0; i < realmDomain.Length; i++)
        {
            realmDomain[i].CountResource();
        }
        //TestMain.AddElement<Domain>(ref realmDomain, new Domain("port"));
        int numberofRoutes = ((realmDomain.Length - 1) * (realmDomain.Length) / 2);
        TradeRoute[] tradeRoute = new TradeRoute[numberofRoutes];
        numberofRoutes = 0;
        for (int i = 0; i < realmDomain.Length; i++)
        {
            for (int j = i + 1; j < realmDomain.Length; j++)
            {
                //Debug.Log("i :" + i + " " + realmDomain[i].GetName() + " -> J :" + j + " " + realmDomain[j].GetName());
                KeyValuePair< float, Tile[]> currPath = TestMain.GetMap().GetDijkstraPath(realmDomain[i].GetCapital(), realmDomain[j].GetCapital(), PathType.CompareTerrain);
                float travelDistance = currPath.Key;
                tradeRoute[numberofRoutes] = new TradeRoute(realmDomain[i], realmDomain[j], travelDistance);
                numberofRoutes++;
            }
        }
        tradeRoute = SortTradeRoutes(tradeRoute);
        for (int i = 0; i < tradeRoute.Length; i++)
        {
            //Debug.Log(i + ": " + tradeRoute[i].GetNames() + ": " + tradeRoute[i].GetDistance());
            tradeRoute[i].TradeBetweenPoints();
        }
        ResourceList tempProd = realmDomain[0].GetProduction();
        ResourceList tempMaint = realmDomain[0].GetMaintenance();
        tempProd.Subtract(tempMaint);
        Debug.Log("net " + tempProd);
        for (int i =0; i < realmDomain.Length; i++)
        {
            Debug.Log(realmDomain[i].GetId());
            for (int j = 0; j < ResourceList.AMOUNT_OF_RESOURCES; j++)
            {
                if (ResourceList.ConvertInt(j) == resource.wheat
                    || ResourceList.ConvertInt(j) == resource.wood)
                {
                    Debug.Log(ResourceList.ConvertInt(j) + " " + realmDomain[i].GetResource(j));
                }
            }
        }
        ResourceList[] resourceTally = new ResourceList[0];
        return resourceTally;
    }
    public TradeRoute[] SortTradeRoutes(TradeRoute[] givenRoute)
    {
        TradeRoute[] tempArray;
        if (givenRoute.Length > 1)
        {
            int[] pivot = { (int)Mathf.Floor(givenRoute.Length / 2), 0, 1 };
            tempArray = new TradeRoute[givenRoute.Length];
            for (int i = 0; i < givenRoute.Length; i++)
            {
                if (i != pivot[0])
                {
                    if (givenRoute[i].GetDistance() < givenRoute[pivot[0]].GetDistance())
                    {
                        //Debug.Log("iteration + " + pivot[1] + " :" + givenRoute[i].GetNames());
                        tempArray[pivot[1]] = givenRoute[i];
                        pivot[1]++;
                    }
                    else
                    {
                        //Debug.Log("if bigger iteration " + (tempArray.Length - pivot[2]) + ": " + givenRoute[i].GetNames());
                        tempArray[tempArray.Length - pivot[2]] = givenRoute[i];
                        pivot[2]++;
                    }
                }
            }

            //Create the two sub arrays
            tempArray[pivot[1]] = givenRoute[pivot[0]];
            TradeRoute[] small = new TradeRoute[pivot[1]];
            TradeRoute[] big = new TradeRoute[pivot[2] - 1];
            //Debug.Log(givepivot[0]);
            for (int i = 0; i < small.Length; i++)
            {
                small[i] = tempArray[i];
            }
            small = SortTradeRoutes(small);
            small.CopyTo(tempArray, 0);
            for (int i = 0; i < big.Length; i++)
            {
                big[i] = tempArray[pivot[1] + 1 + i];
            }
            big = SortTradeRoutes(big);
            big.CopyTo(tempArray, pivot[1] + 1);
        }
        else
        {
            tempArray = givenRoute;
        }
        return tempArray;
    }
}
public class TradeRoute
{
    Domain[] m_points;
    //public string GetNames() { return m_points[0].GetName() + ", " + m_points[1].GetName(); }
    float m_distance;
    public float GetDistance() { return m_distance; }
    public TradeRoute(Domain[] givenDomain, float givenDistance)
    {
        m_points = givenDomain;
        m_distance = givenDistance;
    }
    public TradeRoute(Domain firstDomain, Domain secondDomain, float givenDistance)
    {
        m_points = new Domain[2];
        m_points[0] = firstDomain;
        m_points[1] = secondDomain;
        m_distance = givenDistance;
    }
    public void TradeBetweenPoints()
    {
        for (int i = 0; i < ResourceList.AMOUNT_OF_RESOURCES; i++)
        {
            throw new NotImplementedException();
            /*if (m_points[0].GetName() == "port")
            {
                m_points[0].GiveResource(ResourceList.ConvertInt(i), m_points[0].GetResource(i));
            }
            if (m_points[1].GetName() == "port")
            {
                m_points[1].GiveResource(ResourceList.ConvertInt(i), m_points[0].GetResource(i));
            }
            else
            {
                string test = ResourceList.ConvertInt(i) + "  " + m_points[0].GetName() + ": " + m_points[0].GetResource(i) + ", " + m_points[1].GetName() + ": " + m_points[1].GetResource(i);
                int[] curResource = new int[2];
                curResource[0] = m_points[0].GetResource(i);
                curResource[1] = m_points[1].GetResource(i);
                if (curResource[0] > 0 && curResource[1] < 0)
                {
                    //Give Resources
                    //if some leftover
                    if (curResource[0] + curResource[1] > 0)
                    {
                        m_points[0].TakeResource(ResourceList.ConvertInt(i), curResource[1] * -1);
                        m_points[1].GiveResource(ResourceList.ConvertInt(i));
                    }
                    else
                    {
                        m_points[0].TakeResource(ResourceList.ConvertInt(i));
                        m_points[1].GiveResource(ResourceList.ConvertInt(i), curResource[0]);
                    }

                }
                else if (curResource[0] < 0 && curResource[1] > 0)
                {
                    if (curResource[0] + curResource[1] < 0)
                    {
                        m_points[0].GiveResource(ResourceList.ConvertInt(i), curResource[1]);
                        m_points[1].TakeResource(ResourceList.ConvertInt(i));
                    }
                    else
                    {
                        m_points[0].GiveResource(ResourceList.ConvertInt(i));
                        m_points[1].TakeResource(ResourceList.ConvertInt(i), curResource[0] * -1);
                    }
                }
                if (ResourceList.ConvertInt(i) == resource.wheat || ResourceList.ConvertInt(i) == resource.wood)
                {
                    test += "\n" + m_points[0].GetName() + ": " + m_points[0].GetResource(i) + ", " + m_points[1].GetName() + ": " + m_points[1].GetResource(i);
                    Debug.Log(test);
                }
            }*/
        }
    }
}

public class LinearEquation
{
    public LinearEquation()
    {
        /*Market[] market = new Market[4];
        int[] maintenance = new int[4]; //this is the maintenance to each other market in the network
                                        //from the market you're currently buying from

        quicksort(market, maintenance);
        //path[0] is the shortest

        M* maintenanceModifier[0] > market[0]
        M* maintenanceModifier[0] +M * maintenanceModifier[1] > market[1]
        M* maintenanceModifier[0] +M * maintenanceModifier[1] + M * maintenanceModifier[2] > market[2]
        M* maintenanceModifier[0] +M * maintenanceModifier[1] + M * maintenanceModifier[2] + M * maintenanceModifier[3] > market[3]
        maintenanceModifier[0] > maintenanceModifier[1] > maintenanceModifier[2] > maintenanceModifier[3] > 0
        maintenanceModifier[0] + maintenanceModifier[1] + maintenanceModifier[2] + maintenanceModifier[3] = 1
        actualmaintenance = maintenanceModifier[0] * maintenance[0] + maintenanceModifier[1] * maintenance[1] + maintenanceModifier[2] * maintenance[2] + maintenanceModifier[3] * maintenance[3]
        */
    }
}

