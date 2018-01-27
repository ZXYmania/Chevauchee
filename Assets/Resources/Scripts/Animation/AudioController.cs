using UnityEngine;
using System.Collections;

public class AudioController 
{
	AudioClip[] m_sound;
	// Use this for initialization
	void Start () 
	{
		Object[] temp = Resources.LoadAll("Sounds");
		m_sound = new AudioClip[temp.Length];
		for(int i = 0; i < m_sound.Length; i++)
		{
			m_sound[i] = temp[i] as AudioClip;
		}
	}
	
	// Update is called once per frame
	public AudioClip GetAudio(string givenString)
	{
		for (int i =0; i < m_sound.Length; i++) 
		{
			if(givenString == m_sound[i].name)
			{
				return m_sound[i];
			}
		}
        throw new NonExistantKeyError("Can't find the sound clip "+givenString);
	}
}
