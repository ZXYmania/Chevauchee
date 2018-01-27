using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationOutofBoundsException : System.Exception
{
        public AnimationOutofBoundsException(string message) : base(message)
    {

    }
}

public class AnimationLayer 
{
	string m_name;
    protected string GetAnimationName(string givenAnimationName ,int givenAnimationVariant) { return givenAnimationName + m_colour+givenAnimationVariant; }
    public string GetName() { return m_name; }
	string currentAnimation;
	string nextAnimation;
    string baseAnimation;
	int currentFrame;
	int currentTime;
	bool m_pause;
    bool m_visible;
    Color m_colour;
	SpriteRenderer m_renderer;
    //All of the different animations
	public Dictionary<string,PlayAnimation> m_animationList;
    //Each Gameobject has it's own Layer
	public GameObject GetLayer()
    {
        if (m_animationList.Count < 0)
        {
            throw new AnimationOutofBoundsException("Animation is not set");
        }
        return m_renderer.gameObject;
    }
    //The name of the layer
    //The amount of animations
	public int GetAnimationListSize(){return m_animationList.Count;}
	public string GetAnimation() {return currentAnimation;}
    //Toggle the pause the animation
	public void TogglePause() {m_pause = !m_pause;}
    //Set the pause
	public void SetPause(bool givenToggle) {m_pause = givenToggle;}
	public void SetVisible(bool givenVisible)
    {
        m_visible = givenVisible;
        currentFrame = 0;
        if (m_visible)
        {
            SetTexture(m_animationList[currentAnimation].GetSprite(currentFrame));
        }
        else
        {
            SetTexture(null);
        }
    }
    public bool GetVisible() { return m_visible; }
	public int GetAnimationFrame() 	
	{
		return currentFrame;
	}
	public void SetTimeOffset(string animaton, float givenvalue){m_animationList[animaton].SetTimeOffset(givenvalue);}
	public void SetTimeOffset(float givenValue) {m_animationList[currentAnimation].SetTimeOffset(givenValue);}
	public float GetTimeOffset(){return m_animationList[currentAnimation].GetTimeOffset();}
	public int GetAnimationFrame(int index) {return currentFrame;}
	public void SetTexture(Sprite givenTexture){m_renderer.sprite = givenTexture;}
	public void SetNextAnimation(string givenAnimation){nextAnimation = givenAnimation;}

    public void AddAnimation(string givenAnimation)
    {
        Dictionary<string, PlayAnimation> tempanimationList = TextureController.GetAnimations(givenAnimation, m_colour);
        foreach (var item in tempanimationList)
        {
            if (item.Value.GetTimeOffset() < 1)
            {
                item.Value.SetTimeOffset(1);
            }
            m_animationList.Add(item.Key,item.Value);
        }
    }
    public void ChangeAnimation(string givenAnimationName, int givenAnimationVariant)
    {
        string givenAnimation = GetAnimationName(givenAnimationName, givenAnimationVariant);
        if (currentAnimation != givenAnimation)
        {
            currentFrame = 0;
            currentTime = 0;
        }
        currentAnimation = givenAnimation;
        nextAnimation = baseAnimation;
    }
    public void ChangeAnimation(string givenAnimationName, int givenAnimationVariant, int givenFrame = 0) 
	{
        string givenAnimation = GetAnimationName(givenAnimationName, givenAnimationVariant);
		if(currentAnimation != givenAnimation)
		{
			currentTime = 0;
		}
		currentAnimation = givenAnimation; 
		nextAnimation = baseAnimation;
		currentFrame = givenFrame;
	}
	public void ChangeAnimation()
	{
		currentAnimation = nextAnimation;
		nextAnimation = baseAnimation;
		currentFrame = 0;
		currentTime = 0;
	}

	public void SetAnimationSize(float givenSize){m_renderer.gameObject.transform.localScale = new Vector3 (givenSize, givenSize, 1);}
	public Vector2 GetAnimationSize()		 {return m_animationList[currentAnimation].GetSprite(0).rect.size;}
	public Vector2 GetDefaultAnimationSize() {return m_animationList[baseAnimation].GetSprite(0).rect.size;}
	public void SetAnimationPosition(int givenX, int givenY, int givenZ = 0) {m_renderer.gameObject.transform.localPosition = new Vector3 (givenX, givenY, givenZ);}
    public void Initialise(SpriteRenderer givenRenderer, string givenName, string startingAnimation, Color givenColour, int givenAnimationVariant, bool givenToggle)
    {
        m_name = givenName;
        m_renderer = givenRenderer;
        m_pause = givenToggle;
        currentFrame = 0;
        currentTime = 0;
        m_visible = true;
        m_colour = givenColour;
        m_animationList = new Dictionary<string, PlayAnimation>();
        currentAnimation = GetAnimationName(startingAnimation, givenAnimationVariant);
        nextAnimation = GetAnimationName(startingAnimation, givenAnimationVariant);
        baseAnimation = GetAnimationName(startingAnimation, givenAnimationVariant);
        AddAnimation(startingAnimation);
    }

    public void Animate()
	{
        if (m_visible)
        {
            if (!m_pause)
            {
                //If the time is on speed with the animation
                if (currentTime >= GetTimeOffset())
                {
                    currentFrame++;
                    if (m_animationList[currentAnimation].UpdateFrame(currentFrame))
                    {
                        ChangeAnimation();
                    }
                    currentTime = 0;
                }
                currentTime++;
            }
            if (GetAnimation() == null)
            {
                throw new AnimationOutofBoundsException("The animation isn't set for " + GetName());
            }
            SetTexture(m_animationList[currentAnimation].GetSprite(0));
        }
    }

}
