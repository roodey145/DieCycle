using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// This class should be associated with the player.
/// The purpose of this class is to update the progress of the 
/// accepted job when made...
/// </summary>

public class GUIManager : MonoBehaviour
{

    // Private reference variables
    private TextMeshProUGUI _currentJob, _progress;
    private Slider _progressBar;
    private JobManager _jobManager;


    private void Start()
    {
        // Get the required references to the text fields 
        _currentJob = GameObject.Find("CurrentJob").GetComponent<TextMeshProUGUI>();
        _progress = GameObject.Find("Progress").GetComponent<TextMeshProUGUI>();

        // Get the job progress slider
        _progressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();

        // Get the job manager script that is associated with the player
        _jobManager = GameObject.FindGameObjectWithTag("Player").GetComponent<JobManager>();

        // Hide the job progress interface
        activeDeactiveProgressMenu(false);
    }

    /// <summary>
    /// Update the progress of the accepted job
    /// </summary>
    public void UpdateUI()
    {
        // Check if there is an active job
        if(_jobManager.AssingedJob != null)
        {// There is an active job
            // Activite the interfaces
            activeDeactiveProgressMenu(true);

            _currentJob.text = _jobManager.AssingedJob.JobTitle;
            _progress.text = string.Format("Progress: {0}", _jobManager.AssingedJob.CollectedAmount);
            _progressBar.maxValue = _jobManager.AssingedJob.Amount;
            _progressBar.value = _jobManager.AssingedJob.CollectedAmount;
        }
        else
        {// No active job
            // Hide the progress interfaces
            activeDeactiveProgressMenu(false);
        }
        
    }

    private void activeDeactiveProgressMenu(bool activeState)
    {
        // Hide the interfaces
        _currentJob.gameObject.SetActive(activeState);
        _progress.gameObject.SetActive(activeState);
        _progressBar.gameObject.SetActive(activeState);
    }
}
