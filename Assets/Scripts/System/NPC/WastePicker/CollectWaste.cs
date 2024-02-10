using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectWaste : MonoBehaviour
{
    private bool _collectingGarbage = false;
    private bool _emptyPosition = false;
    private List<GameObject> garbageInVision = new List<GameObject>();

    [Header("Collecting Settings")]
    [Range(3, 7)]
    public int minGarbageCollection = 3; // Min amount of garbage to collect
    [Range(8, 12)]
    public int maxGarbageCollection = 10; // Max amount of garbage to collect

    private int _collectAmount; // The amount of garbage to be collected
    private int _collectedAmount = 0; // The collected amount of garbage

    [Header("Time Settings")]
    [Range(2f, 15f)]
    public float collectingDelay = 5f; // Time between each collect
    [Range(1f, 5f)]
    public float collectingDelayRandomness = 3f; // Randomness for the wait time between collecting 


    private float _collectTimer; // The time left to collect the next garabge

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the NPC has arrived to a GarbagePosition and there is garbage around
        if(_collectingGarbage && garbageInVision.Count > 0 && _collectedAmount < _collectAmount)
        {
            
            if(_collectTimer <= 0)
            {
                // Used to collect the rarity of the garbage
                int dropRate = Random.Range(0, 100);

                // Show collecting animation

                int objectIndexToBeDestroyed = 0;
                // Choose one of the garbage to remove
                for(int i = 0; i < garbageInVision.Count; i++)
                { // This method is used to avoid collecting the same object as the player
                  // Collecting the same object as the player can cause the player to freeze permenantly 
                    if(garbageInVision[i] != null && !garbageInVision[i].GetComponent<Garbage>().isCollecting())
                    {
                        // This object is not being collected by the player
                        objectIndexToBeDestroyed = i;
                        break;
                    }
                }

                Destroy(garbageInVision[objectIndexToBeDestroyed]); // Remove the element from the game area
                garbageInVision.RemoveAt(objectIndexToBeDestroyed); // Remove the object from the collection

                // Raise the amount of the collected pieces
                _collectedAmount++;

                _collectTimer = calcCollectDelay();
                // Start the searching animation
            }
            else
            {
                // Decrease the 
                _collectTimer -= Time.deltaTime;
            }
        }
        // Check if the player reached an empty GarbagePosition
        else if(_collectingGarbage && (garbageInVision.Count == 0 || _collectedAmount >= _collectAmount))
        {
            // Remove this collection position if it is empty
            if(garbageInVision.Count == 0)
            {
                _emptyPosition = true;
            }


            //Debug.LogError("Entering here");
            // Stop collecting garbage
            _collectingGarbage = false;
        }
        else
        {
            //Debug.LogError("\nGarbage in vision: " + garbageInVision.Count 
            //                + "\nCollected garbage: " + _collectedAmount
            //                + "\nCollect garbage: " + _collectAmount
            //                + "\nCollecting garbage: " + collectingGarbage);
        }
    }

    /// <summary>
    /// Choose a random collecting time and amount of pieces to be collected
    /// then starts collecting garbage.
    /// </summary>
    public void startGarbageCollecting()
    {
        _collectingGarbage = true;

        _emptyPosition = false;

        // Reset collected amount
        _collectedAmount = 0;

        // Calculate the time to collect the next piece
        _collectTimer = calcCollectDelay();

        // Choose a random number of garbage to be collected
        _collectAmount = randomCollectingAmount();

        //  Debug.Log("CollectingTimer: " + _collectTimer + "\nCollect Amount: " + _collectAmount);
    }

    /// <summary>
    /// To check if the NPC is collecting garbage
    /// </summary>
    /// <returns>Whether the NPC is collecting garbage or not</returns>
    public bool isCollecting()
    {
        return _collectingGarbage;
    }

    public bool isEmptyPosition()
    {
        return _emptyPosition;
    }

    private float calcCollectDelay()
    {
        // Calculate the delay to collect the next garbage
        return collectingDelay + Random.Range(0f, collectingDelayRandomness);
    }

    /// <summary>
    /// Choose a random number between the minGarbageCollection and 
    /// macGarbageCollection. This number will be used to indicate 
    /// how many pieces of garbage should be collected.
    /// </summary>
    /// <returns>a random number betweem minGarbageCollection and maxGarbageCollection</returns>
    private int randomCollectingAmount()
    {
        return Random.Range(minGarbageCollection, maxGarbageCollection + 1);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Garbage"))
            // Add the object to the collection
            garbageInVision.Add(other.gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Garbage"))
            // Remove the object from the collection
            garbageInVision.Remove(other.gameObject);
    }
}
