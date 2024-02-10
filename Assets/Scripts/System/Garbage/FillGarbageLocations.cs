using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGarbageLocations : MonoBehaviour
{
    //[Header("Garbage Settings")]
    //[Range(1, 10000)]
    //public int garbagePerLocation = 100;
    //[Range(1, 100)]
    //public int garbageAmountRandomness = 10;


    private GameObject[] garbageLocations;

    private GameObject[] garbage;

    private int _LOWEST_DROPRATE = 1;
    private int _HIGHEST_DROP_RATE = 100;
    private float _GARBAGE_Y_POSITION = 0.5f;
    private string _GARBAGE_TAG = "Garbage";
    private string _TOXIC_GARBAGE_TAG = "ToxicGarbage";
    private string _GARBAGE_LOCATIONS_TAG = "GarbageLocation";

    // Start is called before the first frame update
    void Start()
    {
        // Find all the garbage locations
        garbageLocations = GameObject.FindGameObjectsWithTag(_GARBAGE_LOCATIONS_TAG);

        // Find the garbage(models) that can be thrown
        // garbage = GameObject.FindGameObjectsWithTag(_GARBAGE_MODELS_TAG);
        garbage = Resources.LoadAll<GameObject>("Prefabs/Trash");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// This method will fill the garbage location with new garbage. 
    /// The method will run across the locations and depends on the drop
    /// rate of the existing garbage it will drop some of them in the range
    /// of the boxCollider of the garbage location gameobject.
    /// </summary>
    public void fillGarbageLocations()
    {
        // Run accros all the garpage locations
        for(int locationI = 0; locationI < garbageLocations.Length; locationI++)
        {
            // Get the amount of garbage inside the location
            int garbageInTheLocation = garbageLocations[locationI].transform.childCount;

            // Get the dumpsite information
            GarbageLocationInfo dumpSiteInfo = garbageLocations[locationI].GetComponent<GarbageLocationInfo>();

            // Get the min amount of garbage to be added
            int garbageToBeAdded = Random.Range(
                            dumpSiteInfo.garbagePerLocation - garbageInTheLocation
                            , dumpSiteInfo.garbagePerLocation - garbageInTheLocation + dumpSiteInfo.garbageAmountRandomness);


            

            // Add the garbage to the location
            for (int garbagePiece = 0; garbagePiece < garbageToBeAdded; garbagePiece++)
            {

                // Get the drop rate 1-100
                int dropRate = Random.Range(_LOWEST_DROPRATE, _HIGHEST_DROP_RATE);

                // To save all the garbage that has lower dropRate than the chosen one
                List<GameObject> garbageBelowRate = new List<GameObject>();


                // Get all the garbage with lower or equal dropRate than the random chosen one
                for (int i = 0; i < garbage.Length; i++)
                {
                    if (_HIGHEST_DROP_RATE - garbage[i].GetComponent<Garbage>().dropRate <= dropRate)
                    {
                        // This has a lower or equal droprate
                        garbageBelowRate.Add(garbage[i]);
                    }
                }
                

                // Choose a garbage piece to be added
                GameObject newGarbagePiece = Instantiate(
                        garbageBelowRate[Random.Range(0, garbageBelowRate.Count)],
                        transform.position,
                        Quaternion.identity,
                        garbageLocations[locationI].transform
                    );

                // Get the size of the area in which the object can be placed in
                BoxCollider locationArea = garbageLocations[locationI].GetComponent<BoxCollider>();

                float width = locationArea.size.x;
                float length = locationArea.size.z;

                // Choose a random location to the new garbage within the range of the parent
                float x = Random.Range(-width / 2, width / 2);
                float z = Random.Range(-length / 2, length / 2);
                newGarbagePiece.transform.localPosition = 
                    new Vector3(x, -_GARBAGE_Y_POSITION/* + newGarbagePiece.transform.localScale.y/2*/, z);

                switch(dumpSiteInfo.garbageType)
                {
                    case GarbageType.NORMAL:
                        newGarbagePiece.tag = _GARBAGE_TAG; // Indicate that this garbage is in a clean dumpsite
                        break;

                    case GarbageType.TOXIC:
                        newGarbagePiece.tag = _TOXIC_GARBAGE_TAG; // Indicate that this garbage is in a toxic dumpsite
                        break;
                }
                


                // Active the gameobject
                newGarbagePiece.SetActive(true);

            }

            // Indicate that the location is not empty
            garbageLocations[locationI].SetActive(true); // Display the object and its children


        }
    }
}
