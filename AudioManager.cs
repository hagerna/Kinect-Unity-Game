using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//array of sound effects & music
	public Sound[] sounds;
	private static AudioManager instance;

	// Ensure that only one Audio Manager exists
   void Awake()
    {
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

	// Initialize the Sound clips
        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
    }

	// Play a Sound by name, ex. Play("Song1")
	public void Play (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.Play();
	}

	// Stop a Sound by name, ex. Stop("Song1")
	public void Stop (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.Stop();
	}
}
