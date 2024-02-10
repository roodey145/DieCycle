using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractorMenuManager : MonoBehaviour
{
    private bool isOpen;
    [SerializeField]
    private GameObject jobButtonPrefab;
    private GameObject _jobViewerMenu;
    public ContractorManager contractorManager;
    private JobManager _playerJobManager;

    void Start()
    {
        _jobViewerMenu = GameObject.Find("JobViewer");
        _playerJobManager = GameObject.FindGameObjectWithTag("Player").GetComponent<JobManager>();

        foreach (Job job in contractorManager.contractor.Jobs)
        {
            GameObject newButton = Instantiate(jobButtonPrefab);
            newButton.GetComponent<Button>().onClick.AddListener(() => AddJob(_playerJobManager, job, newButton));
            newButton.transform.SetParent(_jobViewerMenu.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = job.JobTitle;
        }
    }

    private void AddJob(JobManager jobManager, Job job, GameObject button)
    {
        // Check if there is already an active job
        if(jobManager.AssingedJob == null)
        {// The player has no active job
            // Hide the button
            button.SetActive(false);

            // Receive the job
            jobManager.AssignJob(job);
        }
        
    }

    public void CloseNPCMenu()
    {
        // GameObject.FindGameObjectWithTag("Menu").GetComponent<GUIManager>().UpdateUI();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);

        // Allow the player to control the character again
        PlayerInfo.isStopped = false;
    }
}
