using UnityEngine;
using System.Collections;

public class SecondDimension : MonoBehaviour
{
	public Vector2 GetTransform(){return new Vector2(transform.position.x, transform.position.y);}
    public virtual float GetTransformX() { return transform.position.x; }
    public virtual float GetTransformY() { return transform.position.y; }
	protected void SetTransform(Vector2 givenVector)
	{
		transform.position = new Vector3(givenVector.x, givenVector.y, transform.position.z);
	}
	protected void SetTransform(float givenX, float givenY)
	{
		transform.position = new Vector3( givenX, givenY, transform.position.z);
	}
}
