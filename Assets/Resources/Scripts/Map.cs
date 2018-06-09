using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using static Database;
using System.Threading.Tasks;

public enum PathType
{
    IgnoreAll,
    ImpassableTerrain,
    CompareTerrain,
    RoadOnly,
    NavalRoute,
    
}

public class NoPathException : System.Exception
{
    public NoPathException(string message) : base(message)
    {

    }
}

public class TileOutofBoundsException : System.Exception
{
    public TileOutofBoundsException(string message) : base(message)
    {

    }
}

public class Map
{
    Dictionary<Position, Tile> m_map;
    int MAPWIDTH;
    int MAPHEIGHT;
    public Tile GetTile(Position givenPosition)
    {
        return m_map[givenPosition];
    }
    public static int[] GetDirection(Tile startTile, Tile endTile)
    {
        int[] direction = new int[] { endTile.GetX() - startTile.GetX(), endTile.GetY() - startTile.GetY() };
        return direction;
    }

    public static int[] GetNormalisedDirection(Tile startTile, Tile endTile)
    {
        int[] direction = new int[] { endTile.GetX() - startTile.GetX(), endTile.GetY() - startTile.GetY() };
        if(direction[0] != 0)
        {
            direction[0] = direction[0] / Mathf.Abs( direction[0]);
        }
        if (direction[1] != 0)
        {
            direction[1] = direction[1] / Mathf.Abs(direction[1]);
        }
        return direction;
    }
    public Map()
    {
    }
    public void GenerateMap()
    {
        m_map = new Dictionary<Position, Tile>();
        //Italy Test Map
        /*SpriteMap oceanMap = TextureController.GetAnimations("England_map", Color.black);
        Color[] mapPixel = oceanMap.GetAnimation(0).GetSprite(0).texture.GetPixels();
        MAPWIDTH = (int)oceanMap.GetAnimation(0).GetSprite(0).texture.width;
        MAPHEIGHT = (int)oceanMap.GetAnimation(0).GetSprite(0).texture.height;
        m_map = new Tile[MAPWIDTH][];
        TileTransaction buildMap = TileTransaction.CreateTileTransaction();
        for (int i = 0; i < MAPWIDTH; i++)
        {
            m_map[i] = new Tile[MAPHEIGHT];
        }
        for (int i = 0; i < mapPixel.Length; i++)
        {
            Color currentColour = mapPixel[i];
            int xPos = i % MAPWIDTH;
            int yPos = Mathf.FloorToInt(i / MAPWIDTH);
            xPos--;
            yPos--;
            if (xPos > 0 && yPos > 0 && xPos < MAPWIDTH-2 && yPos < MAPHEIGHT-2)
            {
                LoadTileFromImage();
                buildMap.AddNewItem(xPos, yPos, currentColour);
            }
        }
        buildMap.ExecuteCommand();*/
        CreateMapDataBase();
        CreateMapView();
    }

    public void CreateMapDataBase()
    {
        SpriteMap oceanMap = TextureController.GetAnimations("England_map", Color.black);
        Color[] mapPixel = oceanMap.GetAnimation(0).GetSprite(0).texture.GetPixels();
        MAPWIDTH = (int)oceanMap.GetAnimation(0).GetSprite(0).texture.width;
        MAPHEIGHT = (int)oceanMap.GetAnimation(0).GetSprite(0).texture.height;
        TileTransaction buildMap = TileTransaction.CreateTileTransaction();
        for (int i = 0; i < mapPixel.Length; i++)
        {
            Color currentColour = mapPixel[i];
            int xPos = i % MAPWIDTH;
            int yPos = Mathf.FloorToInt(i / MAPWIDTH);
            xPos--;
            yPos--;
            if (xPos > 0 && yPos > 0 && xPos < MAPWIDTH - 2 && yPos < MAPHEIGHT - 2)
            {
                Tile.Terrain currentTerrain = Tile.Terrain.blank;
                int probability = UnityEngine.Random.Range(0, 10);
                if (probability < 4)
                {
                    currentTerrain = Tile.Terrain.smooth;
                }
                else if (probability < 7)
                {
                    currentTerrain = Tile.Terrain.forest;
                }
                else if (probability < 9)
                {
                    currentTerrain = Tile.Terrain.rough;
                }
                else
                {
                    currentTerrain = Tile.Terrain.mountain;
                }
                buildMap.AddNewTile(xPos-1, yPos-1, currentTerrain);
            }
        }
        buildMap.ExecuteCommand();

    }
    public void CreateMapView()
    {
        TileTransaction buildView = TileTransaction.CreateTileTransaction();
        Position origin = new Position ((int)TestMain.GetCamera().transform.position.x, (int)TestMain.GetCamera().transform.position.y);
        Position[] bound = new Position[2];
        bound[0] = new Position( origin.x - 5, origin.y - 5 );
        bound[1] = new Position (origin.x + 5, origin.y + 5 );
        buildView.SelectBounds(bound,false);
        while(buildView.GetNextRow())
        {
            Position m_position = buildView.GetPosition();
            m_map.Add(m_position, Tile.LoadTile( m_position, buildView.GetTerrain(), Guid.Empty));
        }
        Task.Run(() => ExtendBound(bound));
    }

    private void ExtendBound(Position[]bound)
    {
        TileTransaction currentBounds = TileTransaction.CreateTileTransaction(); 
        if(bound.Length!=2)
        {
            currentBounds.SelectBounds(bound, true);
        }
        while(currentBounds.GetNextRow())
        {
            Position m_position = currentBounds.GetPosition();
            m_map.Add(m_position, Tile.LoadTile(m_position, currentBounds.GetTerrain(), currentBounds.GetDomain()));
        }
        throw new NonExistantKeyError("You used the wrong amount of bounds");
    }
   

    public Tile[] GetBestPath(Tile startTile, Tile endTile, PathType givenPathType)
    {
        if (givenPathType == PathType.IgnoreAll)
        {
            SearchAbleArray<Tile> bestPath = new SearchAbleArray<Tile>();
            int[] direction = GetDirection(startTile, endTile);
            bestPath.Add(startTile);
            Tile lastItem = startTile;
            if (Mathf.Abs(direction[0]) > Mathf.Abs(direction[1]))
            {
                for (int i = 0; i < Mathf.Abs(direction[0]); i++)
                {
                    Position nextTilepos = bestPath.GetLastItem().GetPosition();
                    lastItem = GetTile(nextTilepos);
                    //formatting from float to int hack
                    if ((i < (Mathf.Abs((float)direction[1] / 2)) || i >= (Mathf.Abs((float)direction[0]) - (Mathf.Abs((float)direction[1]) / 2))))
                    {
                        //go diagonal in direction[0]
                        nextTilepos.x += direction[0] / (int)Mathf.Abs(direction[0]);
                        nextTilepos.y += direction[1] / (int)Mathf.Abs(direction[1]);
                        if (direction[1] % 2 == 1 && Mathf.Abs(direction[0]) - i < 1)
                        {
                            direction[1] -= direction[1] / (int)Mathf.Abs(direction[1]);
                        }
                    }
                    else
                    {
                        //go in direction[0]
                        nextTilepos.x += direction[0] / (int)Mathf.Abs(direction[0]);
                    }
                    bestPath.Add(GetTile(nextTilepos));
                }
                return bestPath.GetOrderedArray();
            }
            else //if (difference < 0)
            {

                Position nextTilepos = bestPath.GetLastItem().GetPosition();
                for (int i = 0; i < Mathf.Abs(direction[1]); i++)
                {
                    if ((i < (Mathf.Abs((float)direction[0] / 2)) || i >= (Mathf.Abs((float)direction[1])- (Mathf.Abs((float)direction[0]) / 2))))
                    {
                        //go diagonal in direction[1]
                        nextTilepos.x += direction[0] / (int)Mathf.Abs(direction[0]);
                        nextTilepos.y += direction[1] / (int)Mathf.Abs(direction[1]);
                        if (direction[0] % 2 == 1 && Mathf.Abs(direction[0]) -i < 1)
                        {
                            direction[0] -= direction[0] / (int)Mathf.Abs(direction[0]);
                        }
                    }
                    else
                    {
                        //go in direction[1]
                        nextTilepos.y += direction[1] / (int)Mathf.Abs(direction[1]);
                    }
                    bestPath.Add(GetTile(nextTilepos));

                }
            }
            return bestPath.GetOrderedArray();
        }
        else
        {
            KeyValuePair<float, Tile[]> output = GetDijkstraPath(startTile, endTile, givenPathType);

            return output.Value;

        }
    }


    public KeyValuePair<float, Tile[]> GetDijkstraPath(Tile source, Tile destination, PathType givenPathType)
    {
        bool pathFound = false;
        int count = 0;
        Tile[] bestPath = new Tile[0];
        float distance = -1;
        float[][] distanceMap = new float[MAPWIDTH][];
        Position?[][] previousTilePos = new Position?[MAPWIDTH][];
        SearchAbleArray<Tile> queue = new SearchAbleArray<Tile>();
        SearchAbleArray<Tile> diagonalQueue = new SearchAbleArray<Tile>();
        SearchAbleArray<Tile> sidewaysQueue = new SearchAbleArray<Tile>();
        SearchAbleArray<Tile> backwardsQueue = new SearchAbleArray<Tile>();
        TileArray explored = new TileArray();
        queue.Add(source);
        for (int i = 0; i < MAPWIDTH; i++)
        {
            distanceMap[i] = new float[MAPHEIGHT];
            previousTilePos[i] = new Position?[MAPHEIGHT];
            for(int j =0; j < MAPHEIGHT; j++)
            {
                distanceMap[i][j] = -1;
                previousTilePos[i][j] = null;
            }
        }
        previousTilePos[source.GetX()][source.GetY()] = new Position(source.GetX(), source.GetY());
        distanceMap[source.GetX()][source.GetY()] = 0;
        while ((queue.GetSize() > 0 || sidewaysQueue.GetSize() > 0 || backwardsQueue.GetSize() > 0) && !pathFound)
        {
            Tile[] currQueueArray;
            SearchAbleArray<Tile> currentQueue;
            if (queue.GetSize() > 0)
            {
                currentQueue = queue;
            }
            else if (diagonalQueue.GetSize() > 0)
            {
                currentQueue = diagonalQueue;
            }
            else if (sidewaysQueue.GetSize() > 0)
            {
                currentQueue = sidewaysQueue;
            }
            else
            {
                currentQueue = backwardsQueue;
            }
            currQueueArray = currentQueue.GetOrderedArray();
            Tile currentElement = currQueueArray[0];
            float minQueueDistance = distanceMap[currQueueArray[0].GetX()][currQueueArray[0].GetY()];
            for (int i = 0; i < currQueueArray.Length; i++)
           {
                if (distanceMap[currQueueArray[i].GetX()][currQueueArray[i].GetY()] < minQueueDistance )
                {
                    currentElement = currQueueArray[i];
                }
            }
            explored.AddTile(currentElement);
            if (currentElement == destination)
            {
                pathFound = true;
            }
            else
            {
                Tile[] queueAdjacent = GetAdjacent(currentElement, destination);

                for (int i = 0; i < queueAdjacent.Length; i++)
                {
                    float currToNextTravelDistance = GetAdjacentTravelDistance(currentElement, queueAdjacent[i], givenPathType);
                    if (currToNextTravelDistance > 0)
                    {
                        if (previousTilePos[queueAdjacent[i].GetX()][queueAdjacent[i].GetY()] == null)
                        {
                            if (i < 1)
                            {
                                queue.Add(queueAdjacent[i]);
                            }
                            else if(i < 3)
                            {
                                diagonalQueue.Add(queueAdjacent[i]);
                            }
                            else if (i < 5)
                            {
                                sidewaysQueue.Add(queueAdjacent[i]);
                            }
                            else
                            {
                                backwardsQueue.Add(queueAdjacent[i]);
                            }
                           float alteredDistance = distanceMap[currentElement.GetX()][currentElement.GetY()] + currToNextTravelDistance;
                            if (distanceMap[queueAdjacent[i].GetX()][queueAdjacent[i].GetY()] < 0 || alteredDistance < distanceMap[queueAdjacent[i].GetX()][queueAdjacent[i].GetY()])
                            {
                               distanceMap[queueAdjacent[i].GetX()][queueAdjacent[i].GetY()] = alteredDistance;
                               previousTilePos[queueAdjacent[i].GetX()][queueAdjacent[i].GetY()] = new Position(currentElement.GetX(), currentElement.GetY());
                            }
                        }
                    }
                }
            }
            currentQueue.Remove(currentElement);
        }
        Tile[] tempBestPath = new Tile[0];
        Tile previousTile = destination;
        do
        {
            if (previousTilePos[previousTile.GetX()][previousTile.GetY()] != null)
            {
                TestMain.AddElement<Tile>(ref tempBestPath, previousTile);
                previousTile = GetTile((Position)previousTilePos[previousTile.GetX()][previousTile.GetY()]);
            }
            else
            {
                throw new NoPathException("There is no path between " + source+ " and " + destination + " when travelling by " + givenPathType);
            }
        }
        while (previousTile != source);
        if(tempBestPath.Length > 5)
        {
            Debug.Log("cool");
        }
        TestMain.AddElement<Tile>(ref tempBestPath, source);
        bestPath = new Tile[tempBestPath.Length];
        for(int i = tempBestPath.Length - 1; i >= 0;i--)
        {
            bestPath[i] = tempBestPath[i];
        }
        return new KeyValuePair< float, Tile[]>(distance, bestPath);
    }

    private float GetTileDistance(Tile startTile, Tile endTile)
    {
        int[] direction = GetDirection(startTile, endTile);
        direction[0] = Mathf.Abs(direction[0]);
        direction[1] = Mathf.Abs(direction[1]);
        int difference = direction[0] - direction[1];
        if (difference > 0)
        {
            return (direction[0] - direction[1]) +(Mathf.Sqrt(2) * direction[1]);
        }
        else if (difference < 0)
        {
            return (direction[1] - direction[0]) + (Mathf.Sqrt(2) * direction[0]);
        }
        else
        {
            return direction[0] * Mathf.Sqrt(2);
        }

    }

    private float GetAdjacentTravelDistance(Tile firstTile, Tile secondTile, PathType givenPathType)
    {
        if (IsAdjacent(firstTile, secondTile))
        {
            if (firstTile.GetObstacle(givenPathType) < 0 || secondTile.GetObstacle(givenPathType) < 0) { return -1; }
            float baseDistance = 0;
            if (firstTile.GetX() != secondTile.GetX() &&
                firstTile.GetY() != secondTile.GetY())
            { baseDistance = Mathf.Sqrt(2); }
            else { baseDistance = 1; }
            return (((firstTile.GetObstacle(givenPathType) + secondTile.GetObstacle(givenPathType)) / 2) * baseDistance);
        }
        else
        {
            return -1;
        }
    }
    public Tile[] GetAdjacent(Tile givenTile)
    {
        Tile[] adjacentTile = new Tile[0];
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j< 2; j++)
            {
                Position currPos = new Position( i + givenTile.GetX(), j + givenTile.GetY());
                if (GetTile(currPos) != null && !(i == 0 && j == 0))
                {
                    TestMain.AddElement<Tile>(ref adjacentTile, GetTile(currPos));
                }
             }
        }
        return adjacentTile;
    }
    public Tile[] GetAdjacent(Tile givenTile, Tile goalTile)
    {

        Tile[] unSortedAdjacentTile = GetAdjacent(givenTile);
        if(unSortedAdjacentTile.Length < 8)
        {
            return unSortedAdjacentTile;
        }
        Tile[] sortedAdjacentTile = new Tile[8];
        Vector2 goalPos = new Vector2(goalTile.GetX() - givenTile.GetX(), goalTile.GetY() - givenTile.GetY());
        if(goalPos.x!=0)
        {
            goalPos.x = goalPos.x / Mathf.Abs(goalPos.x);
        }
        if (goalPos.y != 0)
        {
            goalPos.y = goalPos.y / Mathf.Abs(goalPos.y);
        }
        int[] tileOrder = new int[0];
        switch ((int)goalPos.x)
        {
            case -1:    switch((int)goalPos.y)
                        {
                            case -1: tileOrder = new int[] { 0,1,3,2,5,4,6,7 };
                            break;
                            case 0:  tileOrder = new int[] { 1,0,2,3,4,6,5,7};
                            break;
                            case 1: tileOrder = new int[] {2,1,4,0,7,3,6,5};
                            break;
                        }
                break;
            case 0: switch ((int)goalPos.y)
                    {
                        case -1:
                            tileOrder = new int[] { 3,0,5,1,6,4,2,7};
                            break;
                        case 0:
                            tileOrder = new int[] { 0,1,2,3,4,5,6,7};
                            Debug.Log("Centre tile is the same as the goaltile");
                            break;
                        case 1:
                            tileOrder = new int[] { 4,2,7,1,6,3,0,5};
                            break;
                    }
                break;
            case 1: switch ((int)goalPos.y)
                    {
                        case -1:
                            tileOrder = new int[] { 5,2,6,0,7,1,4,2};
                            break;
                        case 0:
                            tileOrder = new int[] { 6,5,7,3,4,1,0,2};
                            
                            break;
                        case 1:
                            tileOrder = new int[] {7,4,6,2,5,1,3,0};
                            break;
                    }
                break;
        }
        for(int i = 0; i < tileOrder.Length; i++)
        {
            sortedAdjacentTile[i] = unSortedAdjacentTile[tileOrder[i]];
        }
        return sortedAdjacentTile;
        /*Vector2 goalPos = new Vector2(goalTile.GetX() - givenTile.GetX(), goalTile.GetY() - givenTile.GetY());
        goalPos = new Vector2(goalPos.x % 1, goalPos.y % 1);
        
        if (goalPos.x != 0 && goalPos.y != 0)
        {
            for (int i = 0; i < 9; i++)
            {

            }
        }*/
        /*Tile[] adjacentTile = GetAdjacent(givenTile);
        float[] tileLength = new float[adjacentTile.Length];
        for(int i = 0; i < adjacentTile.Length; i++)
        {
            tileLength[i] = GetTileDistance(adjacentTile[i], goalTile);
        }
        TestMain.QuickSort(tileLength, ref adjacentTile);
        return adjacentTile;*/
    }
    public bool IsAdjacent(Tile givenTile, Tile adjacentTile)
    {
        if (givenTile == null || adjacentTile == null)
        {
            return false;
        }
        else
        {
            return (Mathf.Abs(Mathf.Abs(givenTile.GetX()) - Mathf.Abs(adjacentTile.GetX())) <= 1
                && Mathf.Abs(Mathf.Abs(givenTile.GetY()) - Mathf.Abs(adjacentTile.GetY())) <= 1);
        }
    }
    public Tile[] FillArea(Tile[] outerPoint)
    {
        if (outerPoint.Length > 2)
        {
            TileArray perimeter = new TileArray();
            SearchAbleArray<Tile> corner = new SearchAbleArray<Tile>();
            for (int i = 0; i < outerPoint.Length; i++)
            {
                int nextPoint = i + 1;
                if (nextPoint == outerPoint.Length)
                {
                    nextPoint = 0;
                }
                Tile[] tempPerimeter = GetBestPath(outerPoint[i], outerPoint[nextPoint], PathType.IgnoreAll);
                int precedingIndex;
                int afterIndex;
                if (nextPoint == 0)
                {
                   precedingIndex = outerPoint.Length - 1;
                }
                else
                {
                    precedingIndex = nextPoint - 1;
                }
                if(nextPoint == outerPoint.Length-1)
                {
                    afterIndex = 0;
                }
                else
                {
                    afterIndex = nextPoint + 1;
                }
                while(outerPoint[afterIndex].GetX() == outerPoint[nextPoint].GetX())
                {
                    corner.Add(outerPoint[afterIndex]);
                    if(++afterIndex >= outerPoint.Length)
                    {
                        afterIndex = 0;
                    }
                }
                if ((outerPoint[precedingIndex].GetX() <= outerPoint[nextPoint].GetX() && outerPoint[afterIndex].GetX() <= outerPoint[nextPoint].GetX())
                    || (outerPoint[precedingIndex].GetX() >= outerPoint[nextPoint].GetX() && outerPoint[afterIndex].GetX() >= outerPoint[nextPoint].GetX()))
                {
                    corner.Add(outerPoint[nextPoint]);
                }
                for (int j  = 0; j < tempPerimeter.Length; j++ )
                {
                    //if the end tile just addhave already assigned a corner or not
                    if(j == tempPerimeter.Length-1)
                    {
                        perimeter.AddTile(tempPerimeter[j]);
                    }
                    //else if not the startTile
                    else if (j != 0)
                    {
                        //if it is inline with the following tile or with the endTile
                        int temp1 = tempPerimeter[j].GetX();
                        int temp2 = tempPerimeter[j + 1].GetX();
                        bool temp3 = (tempPerimeter[j].GetX() == tempPerimeter[j + 1].GetX());
                        if (tempPerimeter[j].GetX() == tempPerimeter[j + 1].GetX()
                            || (tempPerimeter[j].GetX() == outerPoint[nextPoint].GetX() && corner.Contains(outerPoint[nextPoint])))
                        {
                            corner.Add(tempPerimeter[j]);
                        }
                            perimeter.AddTile(tempPerimeter[j]);
                    }
                }
            }
            Tile[] area = new Tile[0];
            for (int i = 0; i < perimeter.GetAmountofRows(); i++)
            {
                Tile[] rowArray = perimeter.IterateThroughRows(i);
                for ( int j = 0; j < rowArray.Length; j++)
                {
                    if (!corner.Contains(rowArray[j]))
                    {
                        Tile currTile = rowArray[j];
                        Tile endTile = rowArray[j + 1];
                        while (corner.Contains(endTile))
                        {
                            j++;
                            endTile = rowArray[j+1];
                        }
                        for (int k = currTile.GetY(); k <= endTile.GetY(); k++)
                        {
                            Position currPos = new Position(currTile.GetX(), k);
                            TestMain.AddElement<Tile>(ref area, GetTile(currPos));
                        }
                        
                        j++;
                    }
                    else
                    {
                        TestMain.AddElement<Tile>(ref area, rowArray[j]);
                    }
                }
            }
            return area;
        }
        else
        {
            return null;
        }
    }
    public void AddBuilding(Tile givenTile, Character givenCharacter, string buildingType)
    {
        Building newBuilding;
        switch (buildingType)
        {
            case "Farm":
                newBuilding = new Farm();
                break;
            case "LumberMill":
                newBuilding = new LumberMill();
                break;
            case "City":
                newBuilding = new City();
                break;
            case "Road":
                newBuilding = new Road();
                break;
            default:
                newBuilding = null;
                Debug.Log("Building doesn't exist");
                break;
        }
        if (newBuilding != null && givenCharacter.RulesDomain(givenTile.GetDomain()))
        {
            givenTile.AddBuilding(newBuilding);
            throw new NotImplementedException();
            //DomainDictionary.GetDomain(givenTile.GetDomain()).AddBuilding(givenTile.GetPosition());
        }
        else
        {
            Debug.Log("You can't build " + newBuilding + " at " + givenTile.GetDomain());
        }
    }
}

