using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Animated : SecondDimension
{

    protected string animationName;
    protected float m_animationTimer;
    protected Dictionary<string,AnimationLayer> m_animationLayer;
    protected int GetAmountofAnimationLayers() { return m_animationLayer.Count; }
    protected bool HasTicked() { float tempTimer = m_animationTimer + Time.deltaTime; return (Mathf.Floor(m_animationTimer / 0.04f) < Mathf.Floor(tempTimer / 0.04f)); }

    protected virtual void Initialise()
    {
        m_animationLayer = new Dictionary<string, AnimationLayer>();
    }

    protected virtual bool Animate()
    {
        if (HasTicked())
        {
            foreach (KeyValuePair<string,AnimationLayer> item in m_animationLayer)
            {

                    if(item.Value.GetVisible())
                    {
                        item.Value.Animate();
                    }
            }
        }
        m_animationTimer += Time.deltaTime;
        return true;
    }

    protected void AddAnimationLayer(string givenName,string givenAnimation, Color givenColour, bool isPaused, int givenAnimationVariant = 0)
    {
        if (!m_animationLayer.ContainsKey(givenName))
        {
            AnimationLayer currentAnimation = new AnimationLayer();
            GameObject currentModeObject = new GameObject();
            currentModeObject.transform.parent = gameObject.transform;
            currentModeObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            currentModeObject.transform.localScale = new Vector3(1, 1, 1);
            currentModeObject.name = gameObject.name;
            SpriteRenderer currentRenderer = currentModeObject.AddComponent<SpriteRenderer>();
            currentAnimation.Initialise(currentRenderer, givenName, givenAnimation,  givenColour, givenAnimationVariant, isPaused);
            m_animationLayer.Add(givenName, currentAnimation);
        }
        else
        {
            m_animationLayer[givenName].AddAnimation(givenAnimation);
        }
    }

    public void AddAnimation(string givenLayer, string givenAnimation)
    {
        m_animationLayer[givenLayer].AddAnimation(givenAnimation);
    }
    
    public void ChangeAnimation(string givenLayer,string givenAniamtion, int givenFrame = 0)
    {
        m_animationLayer[givenLayer].ChangeAnimation(givenAniamtion,givenFrame);
    }
}

