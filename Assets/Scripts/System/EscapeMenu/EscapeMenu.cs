using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used in the Game Jam. Only necassary adjustments was made. 
/// </summary>
public class EscapeMenu : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject _escapeMenu;
    private AudioManager _audioManager;

    private void Start()
    {
        _escapeMenu = GameObject.Find("EscapeMenu");
        _escapeMenu.SetActive(false);

        // Get the reference to the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ItemSeller.isMenuOpen)
        {
            if (SceneManager.GetActiveScene().name.Equals("Tutorial"))
            {// The player is doing the tutorial
                if(TutorialController.currentQuest == 3)
                {// This is the volume and mouse speed tutorial
                    if (!isPaused) PauseGame();
                    else ResumeGame();
                }
            }
            else
            {// The player is not doing the tutorial
                if (!isPaused) PauseGame();
                else ResumeGame();


            }
            
        }
    }

    public void ChangeMouseSensitivty(Slider sensitivitySlider)
    {
        TutorialController.isMouseSpeedAdjusted = true;
        PlayerController.mouseSensitivity = 1 + (1 * sensitivitySlider.value);
    }

    public void ChangeAudioVolume(Slider volume)
    {
        TutorialController.isVolumeAdjusted = true;
        _audioManager.adjustAudioVolume(volume.value);
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide the mouse cursor
        Cursor.visible = false;
        _escapeMenu.SetActive(false);
        Time.timeScale = 1; // Start all time based objects
        this.isPaused = false;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Show the mouse cursor
        Cursor.visible = true;

        _escapeMenu.SetActive(true);
        this.isPaused = true;
        Time.timeScale = 0; // Stop all time based objects
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
