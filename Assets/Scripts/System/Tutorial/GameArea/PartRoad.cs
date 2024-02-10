using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Should be attached to an object that helps the player
/// to reach the quest destination.
/// </summary>
public class PartRoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {// The player reached this position

            // Hide this object
            gameObject.SetActive(false);
        }
    }
}
