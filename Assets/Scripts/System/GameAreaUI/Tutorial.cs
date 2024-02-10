using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [Header("Quest Accept Button")]
    public GameObject acceptButton;

    [Header("Quests UI")]
    public GameObject[] questUI;

    [Header("Quests Destination")]
    public GameObject[] destinations;


    // Private reference
    private JobManager _jobManager;

    private int _currentQuest = -1;
    private bool _aMenuWasOpen = false;

    public static bool isTutorial = true;
    public static bool isQuestFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        //showTheMouse();

        // Get the reference to the job manager
        _jobManager = GameObject.Find("Player").GetComponent<JobManager>();

        // Deactivate this game object if it is not the tutorial
        if (!isTutorial)
        {
            gameObject.SetActive(false);
            return; // Make sure none of the quests loads
        }
        loadNextQuest();
    }

    // Update is called once per frame
    void Update()
    {
        if (isQuestFinished)
        {
            loadNextQuest();
        }
        else if(_currentQuest == 2 && _jobManager.AssingedJob != null)
        {// This is the get job quest
            isQuestFinished = true;
        }
        else if(_currentQuest == 3)
        {// This is the collecting quest

            // Check if the quest is finished
            if(_jobManager.AssingedJob == null)
            {// The quest is finished
                isQuestFinished = true; // Indicate the end of the quest
            }

            // Check if the player collected enough garbage
            else if(_jobManager.AssingedJob.Amount <= _jobManager.AssingedJob.CollectedAmount)
            {// The player collected enough toxic garbage
                // Indicate the end of the quest
                isQuestFinished = true;
            }
        }
        else if(_currentQuest == 4)
        {// The deliver garbage quest

            // Check if the quest was finished
            if(_jobManager.AssingedJob == null && destinations[_currentQuest] == null)
            {// The quest was delivered 

                // Indicate the end of the quest
                isQuestFinished = true;
            }
        }
    }

    private void loadNextQuest()
    {
        // Move to the next quest
        _currentQuest++;
        isQuestFinished = false;


        // Check if this was the last quest
        if(_currentQuest >= questUI.Length)
        {// This was the last quest
            return;
        }

        // Show the mouse
        showTheMouse();

        // Show the quest info UI
        questUI[_currentQuest].SetActive(true);

        // Show the accept button
        acceptButton.SetActive(true);

        // Show the destination
        destinations[_currentQuest].SetActive(true);// Is removed by a script component of the destination

        // Stop the time
        Time.timeScale = 0;
    }

    public void startQuest()
    {
        Debug.Log(_currentQuest);
        // Close the current quest UI
        questUI[_currentQuest].SetActive(false);

        // Hide the accept button
        acceptButton.SetActive(false);

        // Active the destination object
        destinations[_currentQuest].SetActive(true);

        // Start the time
        Time.timeScale = 1;

        // Hide the mouse
        hideTheMouse();
    }

    private void hideTheMouse()
    {
        // Check if a menu is open
        if(_aMenuWasOpen)
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
