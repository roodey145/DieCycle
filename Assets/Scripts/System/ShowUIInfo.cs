using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUIInfo : MonoBehaviour
{
    private static GameObject _activeInfoUI;

    [Header("UI")]
    public GameObject infoUIToShow;
    public GameObject closeButton;


    private bool _aMenuWasOpen = false;
    private bool _isOut = true;

    private void Start()
    {
        // Assign th closeUI method to the close button
        closeButton.GetComponent<Button>().onClick.AddListener(closeUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _isOut)
        {
            // Show the mouse
            showTheMouse();

            // Save the info ui of this object as the opened one
            _activeInfoUI = infoUIToShow;

            // Show the UI info
            _activeInfoUI.SetActive(true);
            closeButton.SetActive(true);

            _isOut = false;

            // Stop the time
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_isOut)
        {
            Debug.Log("Out");
            _isOut = true;
        }
    }

    /// <summary>
    /// Hides the opened info UI and the close button. 
    /// </summary>
    private void closeUI()
    {
        hideTheMouse();
        _activeInfoUI.SetActive(false);
        closeButton.SetActive(false);

        // Start the time
        Time.timeScale = 1;
    }


    private void hideTheMouse()
    {
        // Check if a menu is open
        if (_aMenuWasOpen)
        {
            _aMenuWasOpen = false;
            return; // Don't hide the mouse
        }

        // Hide the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void showTheMouse()
    {
        // Check if there is any menu open
        if (Cursor.visible)
            _aMenuWasOpen = true;

        // Show the mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
