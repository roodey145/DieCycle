using UnityEngine;
using System;
public class JobManager : MonoBehaviour
{
    private GUIManager _gUIManager;
    private Job _assignedJob;
    public Job AssingedJob
    {
        get
        {
            return _assignedJob;
        }
        set
        {
            // To Prevent the player from accepting a new job if a job was already accepted
            if (_assignedJob == null || (_assignedJob.Amount >= _assignedJob.CollectedAmount)) this._assignedJob = value;
        }
    }

    private string player = "Player"; // That tag of the job progress menu

    void Start()
    {
        // Find the job progress interface
        _gUIManager = GameObject.FindGameObjectWithTag(player).GetComponent<GUIManager>();
    }


    /* Kan be added as a future work */
    //void Update()
    //{
    //    // If deadline is over, set assignedJob to null
    //    if (_assignedJob != null)
    //    {
    //        if (DateTime.Now < _assignedJob.Deadline)
    //        {
    //            AssingedJob = null;
    //        }
    //    }
    //}


    /// <summary>
    /// Assigns a job to the player.
    /// </summary>
    public void AssignJob(Job job)
    {
        // if the player has no job...
        AssingedJob = job; // ... give him the job
        _gUIManager.UpdateUI();
    }

    public void registerProgress()
    {
        // Check if a job was taking
        if(AssingedJob != null)
        {// A job is active
            // Register the new collected garbage
            AssingedJob.CollectedAmount += 1;

            // Display the progress
            _gUIManager.UpdateUI();
        }
    }

    public void receiveMoney()
    {
        PlayerInfo.money += AssingedJob.Paycheck;

        // Clear the job
        AssingedJob = null;

        // Play the money receive audio

        // Hide the progress menu
        _gUIManager.UpdateUI();

        // Display the new info
        PlayerInterfaceController.displayInfo();
    }

    //public void RemoveCurrentJob()
    //{
    //    if (HasJob) _assignedJob = null;
    //}

    public bool HasJob
    {
        get => _assignedJob != null;
    }
}