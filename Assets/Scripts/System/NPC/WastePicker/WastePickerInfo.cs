using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WastePickerInfo : MonoBehaviour
{
    [Header("Worker Info")]
    public string name = "Amodani Somsalfo"; // Default name. The name will be displaied on the top of the worker
    [Range(13, 60)]
    public int age = 30; // Will be used to slow the collection process down in the future
    public GameObject home; // The home of the worker. To send the worker home at the end of the work time

    [Header("Work Settings")]
    [Range(6f, 12f)]
    public float workHours = 8; // The hours that this ai are going to work
    [Range(4, 16)]
    public int workStart = 7; // When this worker are going to start working


    // Worker stats
    public bool isHome = true;
    public bool isRestTime = true;



    public float workEndTime()
    {
        return workStart + workHours;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
