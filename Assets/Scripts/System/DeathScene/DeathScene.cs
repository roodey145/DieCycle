using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


/// <summary>
/// Should be added to the video player to start a random video
/// </summary>
public class DeathScene : MonoBehaviour
{
    // Private reference variables
    private VideoPlayer _videoPlayer;
    private GameObject _videoView;
    private VideoClip[] _resourcesClips;
    private GameObject _donationMenu;

    // Private value variables
    private string _path = "FactsVideos/";

    // Start is called before the first frame update
    void Start()
    {
        // Get the donation canvas
        _donationMenu = GameObject.Find("DonationMenu");

        // Hide the donation menu
        _donationMenu.SetActive(false);

        // Get the video view
        _videoView = GameObject.Find("VideoView");

        // Get the video player component
        _videoPlayer = GetComponent<VideoPlayer>();

        // Get the clip from the resources folder
        _resourcesClips = Resources.LoadAll<VideoClip>(_path);

        // Choose a random clip and assign it to the video player
        _videoPlayer.clip = _resourcesClips[Random.Range(0, _resourcesClips.Length)];

        // Play the video
        _videoPlayer.Play();

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the video stopped to show the donation canvas
        if(_videoPlayer.isPaused && !_donationMenu.activeInHierarchy)
        {// The video ended and the donation canvas is not active

            // Show the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Hide the video view
            _videoView.SetActive(false);

            // Show the donation canvas
            _donationMenu.SetActive(true);
        }
    }
}
