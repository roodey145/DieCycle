using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizensController : MonoBehaviour
{
    // Private reference variables
    private TimeController _timeController;
    private GameObject[] _wastePickers;

    // Private value variables
    private string _wastePickersTag = "WastePicker";
    private float _currentHour;

    // Start is called before the first frame update
    void Start()
    {
        _timeController = GetComponent<TimeController>();

        // Get the waste pickers
        _wastePickers = GameObject.FindGameObjectsWithTag(_wastePickersTag);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get the time in world time
        _currentHour = _timeController.currentTime();

        // Send the worker home if their work time is over
        controlTheWorker();
    }

    private void controlTheWorker()
    {
        // Check all the worker info
        for(int i = 0; i < _wastePickers.Length; i++)
        {
            // Get the worker info
            WastePickerInfo workerInfo = _wastePickers[i].GetComponent<WastePickerInfo>();

            // Check if the worker is at home
            if(workerInfo.isHome)
            { // The worker is at home
                
                // Check if this is the work time
                if(_currentHour > workerInfo.workStart && _currentHour < workerInfo.workEndTime())
                {
                    // Debug.Log("Work time");
                    //workerInfo.isHome = false; // Should be removed and contrlled by the waste picker ai or animation
                    workerInfo.isRestTime = false;
                }
                // Else continue resting
            }
            else
            { // The worker is not at home

                // Check if this is the rest time
                if(_currentHour > workerInfo.workEndTime() || _currentHour < workerInfo.workStart)
                {
                    // Indicate the rest time
                    workerInfo.isRestTime = true;
                }
                // Else continue working
            }
            // Check all the worker if their work hours is over

        }
    }
}
