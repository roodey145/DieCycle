using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public static int currentQuest = 1;
    private GameObject _currentQuestUI;

    [Header("UI")]
    public GameObject quest1UI;
    public GameObject quest2UI;
    public GameObject quest3UI;
    public GameObject acceptButton;

    [Header("Garbage Quest")]
    public GameObject garbageToShow;

    // // // Quests variables
    // Quest 1 (Movement)
    private bool _movedForaward = false;
    private bool _movedBackward = false;
    private bool _movedToLeft = false;
    private bool _movedToRight = false;

    // Quest 2 (Garbage Collecting)
    private bool _collectedGarbage = false;


    // Quest 3 (Volume and Mouse speed)
    public static bool isMouseSpeedAdjusted = false;
    public static bool isVolumeAdjusted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Show the mouse
        showTheMouse();

        _currentQuestUI = quest1UI;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentQuest == 1)
        { // The movement quest
            // Detected the player movement
            if(Input.GetKey(KeyCode.W))
                _movedForaward = true;
            else if (Input.GetKey(KeyCode.S))
                _movedBackward = true;
            else if (Input.GetKey(KeyCode.D))
                _movedToRight = true;
            else if (Input.GetKey(KeyCode.A))
                _movedToLeft = true;

            // Check if the player completed the quest
            if(_movedForaward && _movedBackward 
                && _movedToLeft && _movedToRight)
            {
                // Indicate the starts of the second test
                currentQuest++;

                // Show the UI for the next test
                _currentQuestUI = quest2UI; // Rememeber the next UI
                _currentQuestUI.SetActive(true); // Active the quest UI
                acceptButton.SetActive(true); // Show the accept button

                // Show the garbage to prepare for the next level
                garbageToShow.SetActive(true);

                // Show the mouse
                showTheMouse();

                // Stop the time
                Time.timeScale = 0;
            }
        }
        else if(currentQuest == 2)
        {// The garbage collecting quest

            // Check if the garbage was collected
            if(garbageToShow == null)
            {// The garbage was collected

                // Indicate the starts of the third test
                currentQuest++;

                // Show the UI for the next test
                _currentQuestUI = quest3UI; // Rememeber the next UI
                _currentQuestUI.SetActive(true); // Active the quest UI
                acceptButton.SetActive(true); // Show the accept button

                // Show the mouse
                showTheMouse();

                // Stop the time
                Time.timeScale = 0;
            }
        }
        else if(currentQuest == 3)
        {// The volume and mouse speed quests

            if(isMouseSpeedAdjusted && isVolumeAdjusted)
            {// The quest is done
                Time.timeScale = 1;

                // Send the player to the next level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void startQuest()
    {
        // Close the current quest UI
        _currentQuestUI.SetActive(false);

        // Hide the accept button
        acceptButton.SetActive(false);

        // Start the time
        Time.timeScale = 1;

        // Hide the mouse
        hideTheMouse();
    }

    private void hideTheMouse()
    {
        // Hide the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void showTheMouse()
    {
        // Show the mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
