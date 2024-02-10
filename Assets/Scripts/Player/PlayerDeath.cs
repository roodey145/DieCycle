using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RestartActionOnDeath
{
    RESTART_FROM_START, RESTART_FROM_LAST_DAY, RESTART_FROM_CURRENT_DAY
}

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    public RestartActionOnDeath actionOnDeath;
    public float deathNotificationMinAlpha;
    public float deathAnimationTime;

    // Priivate reference variables
    private TimeController _timeController;
    private AudioManager _audioManager;
    private Image _deathNotification;

    // Private value variables
    private float _deathAnimationTimer;
    private bool _deathSoundIsPlaying;
    private string _deathAudioName = "Death";

    // Start is called before the first frame update
    void Start()
    {
        _deathSoundIsPlaying = false;
        _deathAnimationTimer = deathAnimationTime;

        // Find the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();


        // Find the death notification and deactivite it 
        //deathNotification = GameObject.Find("DeathNotification").GetComponent<Image>();
        //deathNotification.gameObject.SetActive(false);

        // Get the sound length
        _deathAnimationTimer = _audioManager.getAudioDuration(_deathAudioName);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInfo.isDead)
        {   // The player is dead

            // Check if the audio sound is playing
            if(!_deathSoundIsPlaying)
            {
                // Play the audio
                _audioManager.Play(_deathAudioName);

                // Indicate that the audio is playing
                _deathSoundIsPlaying = true;
            }

            // Count down to the end of the animation
            _deathAnimationTimer -= Time.deltaTime;

            // Check if the animation ended
            if(_deathAnimationTimer <= 0)
                dead();
        }
    }


    private void dead()
    {
        // Play a video/sound that informs the player about some information about poverty
        /* For instance, a voice that says if i could receive a hourly loan i woudn't have dead */

        PlayerInfo.reset(actionOnDeath);


        // Send the player to the next scene which is the death scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //PlayerInfo.energy = 100; // Should be removed later
        //displayHealthInfo();
    }
}
