using UnityEngine.Audio;
using UnityEngine;


// publci class for adding audio clips with adjustable volume, range, and whether it loops
[System.Serializable]
public class Sound
{
    
	public string name;
	
	public AudioClip clip;
	
	[Range(0f,1f)]
	public float volume;
	[Range(.1f,3f)]
	public float pitch;
	
	public bool loop;
	
	[HideInInspector]
	public AudioSource source;
}
