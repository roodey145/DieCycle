using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroController : MonoBehaviour
{
    // Private reference variables
    private VideoPlayer _videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the video player component
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the video stopped
        if (_videoPlayer.isPaused)
        {// The intro video ended

            // Show the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Go to the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
