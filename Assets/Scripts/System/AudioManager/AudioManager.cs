using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used before in other projects. Using the same scrip with adjustments.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] internal Sound[] sounds;

    internal Sound currentSound;
    public static float heartBeat;
    private float walkSoundTimer = 0;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent <AudioSource>();
            s.source.clip = s.clip;
            
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        // Start the sound that should play in the background
        Play("BackgroundNoise");

        // Check if this is the first day
        if(PlayerInfo.currentDay == 1 && !SceneManager.GetActiveScene().name.Equals("Tutorial"))
        {// The game just started

            // Play the dilemma sound
            Play("FirstDayDilemma");
        }
    }

    public void adjustAudioVolume(float volume)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            // Make sure that the sound is not empty
            if(sounds[i] != null)
            {
                // The sound.volume is the initial volume, whereas sound.source.volume is
                // the volume that are being used when playing the audio
                sounds[i].source.volume = sounds[i].volume * volume;

            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s != null)
        {
            s.source.time = 0;
            s.source.Play();
            currentSound = s;
        }
        else
        {
            Debug.Log(name);
        }
    }


    /// <summary>
    /// Stops an audio clip from player if it is playing. 
    /// </summary>
    /// <param name="name">The name of the audio clip</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
    }

    public float getAudioDuration(string name)
    {
        float duration = 0;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null)
        {
            duration = s.clip.length;
        }

        return duration;
    }
}
