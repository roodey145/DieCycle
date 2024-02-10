using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationUIController : MonoBehaviour
{
    [Header("UI To Be Shown")]
    public GameObject UI;// Information UI

    [Header("Close Button")]
    public Button closeButton;
    public bool finishesQuest = false;


    private bool _aMenuIsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        // Add the function to be executed one button is clicked
        closeButton.onClick.AddListener(finishQuest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show the UI
            UI.SetActive(true);

            // Show the close button
            closeButton.gameObject.SetActive(true);

            // Show the mouse
            showTheMouse();

            // Stop the Time
            Time.timeScale = 0;
        }
    }

    private void finishQuest()
    {
        // Hide the UI
        UI.SetActive(false);

        // Hide the close button
        closeButton.gameObject.SetActive(false);

        // Hide the mouse
        hideTheMouse();

        // Run the time again
        Time.timeScale = 1;

        Tutorial.isQuestFinished = finishesQuest;

        // Make sure it is not the job quest
        Destroy(gameObject);
    }


    private void hideTheMouse()
    {
        if(_aMenuIsOpen)
        {
            _aMenuIsOpen = false;
            return;
        }
        // Hide the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void showTheMouse()
    {
        // Check if a menu was already open
        if (Cursor.visible)
            _aMenuIsOpen = true;

        // Show the mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
