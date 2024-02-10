using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Collecting Settings")]
    public string garbageTag = "Garbage";
    public string toxicGarbageTag = "ToxicGarbage";
    [Range(1f, 5f)]
    public float collectingRange = 3;
    public bool showObjectInFocus = true;
    public Color objectInFocusGlowColor = new Color(150, 150, 150);
    [Range(0.1f, 1f)]
    public float objectInFocusScaleIncrease = 0.5f;

    // Private references
    private GameObject _camera; // To cast a ray in the direction of the player vision
    private GameObject _collectNotification;
    private JobManager _jobManager;
    private GameObject _lastDetectedGarbage;


    // Start is called before the first frame update
    void Start()
    {
        // Get the required references
        _camera = transform.Find("MainCamera").gameObject;
        _collectNotification = GameObject.Find("CollectNotification");
        _jobManager = GetComponent<JobManager>();

        // Hide the collect notification
        _collectNotification.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerInfo.isDead && !PlayerInfo.isStopped)
        {
            detectGarbage();
        }

        // The position state can be used to make the main character work slower
    }



    private void detectGarbage()
    {
        // Cast a ray to check if there is object in the vision
        RaycastHit hitInfo;
        Ray forwardRayFromCamera = new Ray(_camera.transform.position, _camera.transform.forward);
        bool isObjectDetected = Physics.Raycast(forwardRayFromCamera, out hitInfo, collectingRange);

        // Check if there is any object detected in the collecting range 
        if(isObjectDetected)
        { // An Object was detected in the forward direction of the camera and in the collecting range

            // Check if the detected object is garbage
            if(hitInfo.collider.CompareTag(garbageTag) || hitInfo.collider.CompareTag(toxicGarbageTag))
            {// The detected object is a garbage

                // Increase the detected object glow to distinguish it from the other items
                // Check if the detected object is the same as the last detected object
                if (showObjectInFocus && hitInfo.collider.gameObject != _lastDetectedGarbage)
                {
                    if(_lastDetectedGarbage != null) 
                    {
                        removeFocusFromLastDetectedGarbage();
                    }
                    // Get the material of the detected object
                    MeshRenderer detectedObjectMR = hitInfo.collider.gameObject.GetComponent<MeshRenderer>();

                    // Enable the Emission
                    detectedObjectMR.material.EnableKeyword("_EMISSION");
                    detectedObjectMR.material.SetColor("_EmissionColor", objectInFocusGlowColor);

                    // Increase the size of the current detected object
                    hitInfo.collider.gameObject.GetComponent<Garbage>().focus(objectInFocusScaleIncrease);

                    // Remember the corrent detected object
                    _lastDetectedGarbage = hitInfo.collider.gameObject;
                    _collectNotification.SetActive(true);

                }
                else if(_lastDetectedGarbage != null && hitInfo.collider.gameObject != _lastDetectedGarbage)
                {   // The last detected garbage is out of the range now
                    removeFocusFromLastDetectedGarbage();
                }
            }
            else if (_lastDetectedGarbage != null)
            {
                removeFocusFromLastDetectedGarbage();
            }
            else
            {
                //Debug.Log(hitInfo.collider.tag);
                // Hide the collect notification
                _collectNotification.SetActive(false);
            }
        }
        else if(_lastDetectedGarbage != null)
        {
            removeFocusFromLastDetectedGarbage();
            // _collectNotification.SetActive(false);
        }
        else if(_collectNotification.activeInHierarchy)
        {
            // Hide the collect notification
            _collectNotification.SetActive(false);
        }


        // Check if the player wants to collect the garbage in focus
        if(Input.GetKey(KeyCode.E) && _lastDetectedGarbage != null)
        {

            // Check if the last detected garbage is toxic garbage
            if(_lastDetectedGarbage.CompareTag(toxicGarbageTag))
            {// Trying to collect toxic garbage

                // Check a quest was accepted and the quest is not completed
                if(_jobManager.AssingedJob != null 
                    && _jobManager.AssingedJob.CollectedAmount < _jobManager.AssingedJob.Amount)
                {
                    // Collect the toxic garbage
                    _lastDetectedGarbage.GetComponent<Garbage>().collect();

                    // Indicate that there is no garbage in focus now
                    _lastDetectedGarbage = null;
                }
            }
            else
            {// Trying to collect clean garbage
                // Collect the garbage in focus
                _lastDetectedGarbage.GetComponent<Garbage>().collect();

                // Indicate that there is no garbage in focus now
                _lastDetectedGarbage = null;
            }
            
        }

    }



    private void removeFocusFromLastDetectedGarbage()
    {
        _collectNotification.SetActive(false);
        // Decrease the glow of the last detected object since it is not in the focus anymore
        MeshRenderer lastDetectedObjectMR = _lastDetectedGarbage.GetComponent<MeshRenderer>();

        lastDetectedObjectMR.material.SetColor("_EmissionColor", Color.black);

        // Return the size of the last detected garbage to its original size
        _lastDetectedGarbage.GetComponent<Garbage>().removeFocus();

        // Set the last detected object to null since it is out of the range now
        _lastDetectedGarbage = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_camera != null)
            Gizmos.DrawRay(new Ray(_camera.transform.position, _camera.transform.forward * collectingRange));
    }
}
