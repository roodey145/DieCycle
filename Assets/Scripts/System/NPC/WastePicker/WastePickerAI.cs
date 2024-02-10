using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WastePickerAI : MonoBehaviour
{
    [Header("Wander Settings")]
    public float destinationDistanceOffset;
    public float hearableDistance;

    private GameObject[] _wanderingDestinations;
    private AudioManager _audioManager;

    [Header("Garbage Picking Settings")]
    public int minGarbageSelectionFromDestination; // Min garbage pieces amount to be selected from each destination
    public int maxGarbageSelectionFromDestination; // Max garbage pieces amount to be selected from each destination


    // Private reference variables
    private CollectWaste _collectWaste;
    private WastePickerInfo _info;
    private NavMeshAgent agent;
    private GameObject _currentTarget;
    private Animator _animator; // Animation_int parameter is used to control the animation
                                // The value 0 indicate the idle state
                                // The value 1 indicate the walk state

    // Private value variables
    //private bool _isRestTime = false;
    // private bool _isHome = false; // Will be used to send the worker home
    private Vector3 _targetPos; // Struct

    private bool _startedCollecting = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Find the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // Assign the wandering destinations
        _wanderingDestinations = findGarbageLocation();

        // Get the collect waste script
        _collectWaste = GetComponent<CollectWaste>();

        // Get the information of this waste picker
        _info = GetComponent<WastePickerInfo>();

        // Indicate that the waste picker is not home anymore
        //_info.isHome = false;

        // Get the animator associated with this game object
        if(!TryGetComponent<Animator>(out _animator))
        {// This object does not have an animator component

            // Deactivite this worker
            gameObject.SetActive(false);
        }

        // Start wandering
        chooseDestination();
        wander();
    }

    /// <summary>
    /// Finds the dumpsites that does not contains any poison. 
    /// </summary>
    /// <returns>A GameObject[] of the clean dumpsites.</returns>
    private GameObject[] findGarbageLocation()
    {
        GameObject[] allGarbageLocations = GameObject.FindGameObjectsWithTag("GarbageLocation");
        GameObject[] cleanGarbageLocations; // Save refernces to the dumpsite that is toxic free

        // To count the toxic location and calc the cleanGarbageLocations array size
        int toxicGarbageCount = 0;
        // Clean the toxic garbage locations
        for (int i = 0; i < allGarbageLocations.Length; i++)
        {
            // Get the dumpsite information
            GarbageLocationInfo locationInfo = allGarbageLocations[i].GetComponent<GarbageLocationInfo>();

            // Check if this location is toxic
            if (locationInfo.garbageType == GarbageType.TOXIC)
            {
                // Indicate that a new toxic location is found
                toxicGarbageCount++;

                // Remove the refernce to this location to avoid adding it to the clean GarbageLocations array
                allGarbageLocations[i] = null;
            }
        }

        // Assign the cleanGarbageLocations size
        cleanGarbageLocations = new GameObject[allGarbageLocations.Length - toxicGarbageCount];

        int cleanGarbageIndex = 0;

        // Add the clean garbage location to the cleanGarbageLocations array
        for (int i = 0; i < allGarbageLocations.Length; i++)
        {
            if(allGarbageLocations[i] != null)
            {// A clean garbage location was found
                // Add the refernce to the clean locations array 
                // And calc the index of the next element
                cleanGarbageLocations[cleanGarbageIndex++] = allGarbageLocations[i];
            }
        }


        return cleanGarbageLocations;
    }
    

    // Update is called once per frame
    void Update()
    {

        if (!PlayerInfo.isFrozen && !PlayerInfo.isDead && !_info.isRestTime)
        {
            // Not Resting
            // Check if the agent stopped
            if (agent.isStopped)
            {
                // Allow the agent to take control over this object again
                agent.isStopped = false;
            }

            // Check if the player is home
            if(_info.isHome)
            {
                // Start Working
                wander();
                _info.isHome = false;
            }

            // Calculate the distance between the target position and this object
            float distance = Vector3.Distance(_targetPos, transform.position);
            if (distance <= destinationDistanceOffset)
            {
                // Check if the NPC started collecting after it reached this garbage position
                if (!_startedCollecting)
                {
                    // Active the idle state. Later the collect animation should be used
                    activeIdleAnimation();

                    _startedCollecting = true; // Indicate the beggining of the collection
                    // Pick garbage
                    _collectWaste.startGarbageCollecting();
                }
                else
                {
                    // Check if the NPC is still collecting
                    if(!_collectWaste.isCollecting())
                    {
                        // Check if this position is empty
                        if(_collectWaste.isEmptyPosition())
                        {
                            // Set this collection position as inactive
                            // Should be activited the next day
                            // _currentTarget.SetActive(false); // This part is changed
                            // Debug.Log("Empty");
                        }
                        _startedCollecting = false; // Indicate the end of the collecting
                        wander();
                    }
                }

                
            }
        }
        else
        {
            //if (!agent.isStopped && PlayerInfo.isStopped)
            //{// The player have opened the Escape menu
            //    // Stop the agent so this object stops moving
            //    agent.isStopped = true;
            //}
            //else if(_info.isRestTime && !_info.isHome)
            {// The worker hours time is over
                
                // Check if the worker is one the way home
                if ((agent.destination - _info.home.transform.position).magnitude > destinationDistanceOffset)
                {
                    // Send the worker home
                    agent.destination = GetComponent<WastePickerInfo>().home.transform.position;

                    // Start the agent 
                    agent.isStopped = false;
                }
                else
                {// The worker is one the way home
                    // Check if he arrived
                    if ((transform.position - _info.home.transform.position).magnitude < destinationDistanceOffset)
                    {// The worker arrived at the diestination || very near of it so it can be descriped as arrived
                        // Active the idle animation
                        activeIdleAnimation();
                        // Indicate that he worker is at home now
                        _info.isHome = true;
                    }
                }
            }
            
        }
    }

    

    private void wander()
    {
        // Activate the movement animation
        activeMoveAnimation();

        // Choose destination
        chooseDestination();

        if(!_info.isRestTime)
            // Assign the destination
            agent.SetDestination(_targetPos);

        // Set the agent to not stopped
        agent.isStopped = false;
    }

    private void chooseDestination()
    {
        // Randomlly selects one of the destinations and then assign its position to targetPos variable
        int element = Random.Range(0, _wanderingDestinations.Length);
        // Get the target
        _currentTarget = _wanderingDestinations[element];

        _targetPos = chooseRandomPositionWithinTarget(_currentTarget);



        int allowedAttempts = _wanderingDestinations.Length;
        int attemptsCount = 0;

        while (!_currentTarget.activeInHierarchy && attemptsCount < allowedAttempts)
        { // The previous selected location is empty

            // Debug.Log("Inactive collection position");
            // Choose a new target location
            element = Random.Range(0, _wanderingDestinations.Length);
            _currentTarget = _wanderingDestinations[element];

            // Choose a random position within the location
            _targetPos = chooseRandomPositionWithinTarget(_currentTarget);

            attemptsCount++;
            // Make sure that this loop doesn't become an infintíte loop
            if(attemptsCount == allowedAttempts)
            {
                // Check if there is any collect position that is not empty
                for(int i = 0; i < _wanderingDestinations.Length; i++)
                {
                    // Check if this collecting position is not empty
                    if(_wanderingDestinations[i].gameObject.activeInHierarchy)
                    {
                        // Set it as the target destination
                        _currentTarget = _wanderingDestinations[i];
                        // Choose a random position within the location
                        _targetPos = chooseRandomPositionWithinTarget(_currentTarget);
                        break;
                    }

                    if(i == _wanderingDestinations.Length - 1)
                    { // All the target position are inactive
                        //_info.isRestTime = true;
                        Debug.Log("RestTime");
                    }
                }
            }
        }
        // Debug.Log("/nElement: " + element + "\nPrev con: " + (attemptsCount < allowedAttempts));

    }


    private Vector3 chooseRandomPositionWithinTarget(GameObject target)
    {
        // Get the size of the target location area
        float width = target.GetComponent<BoxCollider>().size.x;
        float length = target.GetComponent<BoxCollider>().size.z;

        // Get a random position inside the target range
        return target.transform.position + new Vector3(
            Random.Range(-width / 2f, width / 2f), 0, Random.Range(-length / 2f, length / 2f)
            );
    }


    // Animation methods
    private void activeMoveAnimation()
    {
        _animator.SetInteger("Animation_int", 1);
    }

    private void activeIdleAnimation()
    {
        _animator.SetInteger("Animation_int", 0);
    }


    public void startRestTime()
    {
        _info.isRestTime = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_targetPos != null)
            Gizmos.DrawSphere(_targetPos, 1);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
