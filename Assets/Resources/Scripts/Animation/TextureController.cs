using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureNotfoundException : System.Exception
{
    public TextureNotfoundException(string message) : base(message)
    {

    }
}
public static class TextureController 
{
    public static Color transparent = new Color(0, 0, 0, 0);
    // Use this for initialization
    static int XGAP = 1;
    static int YGAP = 1;
	
	private static Dictionary<string, SpriteMap> layerList;
	public static string[] folderPath;
	
	public static void Initialise()
	{
        layerList = new Dictionary<string, SpriteMap>();
		LoadSpriteMaps();
	}
	
	private static void LoadSpriteMaps()
	{
		Color currentColour = transparent;		
		//Get the texture folder
		string tempPath = @"Texture";
		//Load the sprites found at the path
		Object[] tempSpriteMap = Resources.LoadAll(tempPath, typeof(Sprite));
		//For each sprite map
		for(int i = 0; i < tempSpriteMap.Length; i++)
		{
			//Convert the found sprite to sprite
			Sprite currSprite = tempSpriteMap[i] as Sprite;
			//Find this sprites height

			//Make a new layer
            SetAnimations(currSprite ,ref layerList);
           
		}	
	}

    public static string ColourToString(Color givenColour)
    {
        return "#"+givenColour.r + givenColour.b + givenColour.g;
    }

    public static void SetAnimations(Sprite givenSprite, ref Dictionary<string,SpriteMap> givenDictionary)
    {
        Color animationColour = transparent;
        Rect currentAnimationBorder = new Rect(new Vector2(0,0), new Vector2(givenSprite.rect.size.x, 0));
        SpriteMap currentSpriteMap = new SpriteMap();
        string currentSpriteMapId;
        for (int i = 0; i < givenSprite.rect.size.y; i++)
        {
            Color pixelColour = givenSprite.texture.GetPixel(0, i);
            if (animationColour != pixelColour )
            {
                if(pixelColour!= transparent)
                {
                    if(animationColour!= transparent)
                    {
                        currentSpriteMapId = currentSpriteMap.GetName() + ColourToString(currentSpriteMap.GetColour());
                        givenDictionary.Add(currentSpriteMapId, currentSpriteMap);
                    }
                    animationColour = pixelColour;
                    currentAnimationBorder.y = i;
                    currentSpriteMap.Initialise(givenSprite.name, animationColour);
                }
                else
                {
                    currentAnimationBorder.size = new Vector2(currentAnimationBorder.size.x, i - currentAnimationBorder.y);
                    PlayAnimation currentAnimation = SetAnimation(currentAnimationBorder, animationColour, givenSprite.texture);
                    currentSpriteMap.AddAnimation(currentAnimation);
                    i += YGAP - 1;
                    currentAnimationBorder.y = i+1;
                }
            }
            else if( i == givenSprite.rect.size.y-1)
            {
                if(pixelColour == transparent)
                {
                    throw new TextureNotfoundException(givenSprite.name +" didn't find any textures to make an animation from");
                }
                currentAnimationBorder.size = new Vector2(currentAnimationBorder.size.x, i - currentAnimationBorder.y);
                Rect currentFrameBorder = new Rect(currentAnimationBorder.position, new Vector2(0, currentAnimationBorder.size.y));
                PlayAnimation currentAnimation = SetAnimation(currentAnimationBorder, animationColour, givenSprite.texture);
                currentSpriteMap.AddAnimation(currentAnimation);
                i += YGAP - 1;
            }
        }
        currentSpriteMapId = currentSpriteMap.GetName() + ColourToString(currentSpriteMap.GetColour());
        givenDictionary.Add(currentSpriteMapId, currentSpriteMap);
    }
    public static PlayAnimation SetAnimation(Rect currentAnimationBorder, Color animationColour, Texture2D givenTexture)
    {
        PlayAnimation output = new PlayAnimation();
        output.Initialise();
        Rect currentFrameBorder = new Rect(currentAnimationBorder.position, new Vector2(0, currentAnimationBorder.size.y));
        Color pixelColour = transparent;
        for (int j = 0; j < currentAnimationBorder.size.x; j++)
        {
            pixelColour = givenTexture.GetPixel(j, (int)currentFrameBorder.y);
            if (animationColour != pixelColour)
            {
                if (pixelColour == transparent)
                {
                    if (currentFrameBorder.size.x < 1)
                    {
                        currentFrameBorder.size = new Vector2(j, currentFrameBorder.size.y);
                    }
                    Rect imageBorder = new Rect(new Vector2(currentFrameBorder.x + 1, currentFrameBorder.y + 1),
                                                new Vector2(currentFrameBorder.size.x - 2, currentFrameBorder.size.y - 2));
                    output.AddFrame(givenTexture, imageBorder);
                    j += XGAP - 1;
                    currentFrameBorder.x = j + 1;
                }
            }
            else if (j == currentAnimationBorder.size.x - 1)
            {
                if (animationColour != pixelColour && pixelColour != transparent)
                {
                    throw new TextureNotfoundException(givenTexture.name + " didn't find any textures to make a frame from");
                }
                if (currentFrameBorder.size.x < 1)
                {
                   currentFrameBorder.size = new Vector2(j, currentAnimationBorder.size.y);
                   currentFrameBorder = new Rect(new Vector2(currentFrameBorder.x + 1, currentFrameBorder.y + 1),
                                                new Vector2(currentFrameBorder.size.x - 1, currentFrameBorder.size.y - 1));
                }
                output.AddFrame(givenTexture, currentFrameBorder);
                j += XGAP - 1;
                currentFrameBorder.x = j + 1;
            }
            else
            {
                if (currentFrameBorder.size.x > 2)
                {
                    //Debug.Log("skip");
                    j += (int)currentFrameBorder.size.x - 2;
                }
            }
        }
        return output;
    }
	public static int GetTimeOffset()
	{
		return 0;
	}

	public static SpriteMap GetAnimationLayer(string layerName, Color givenColour)
	{
        string id = layerName + ColourToString(givenColour);
        if (layerList.ContainsKey(id))
        {
            return layerList[id];
        }
        throw new TextureNotfoundException("Texture " + layerName + " with a border of " + givenColour + " is not found");

    }

    public static Dictionary<string, PlayAnimation> GetAnimations(string layerName, Color givenColour)
    {
        string id = layerName + ColourToString(givenColour);
        if(layerList.ContainsKey(id))
        {
            return layerList[id].GetAnimations();
        }
        throw new TextureNotfoundException("Texture " + layerName + " with a border of " + givenColour + " is not found");
    }


}
