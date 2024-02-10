using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should be associated with a gameobject that has 
/// collider and a rigid body. If the player enters bunderies of
/// the box collider, the player will be poisoned.
/// </summary>

public class ToxicArea : MonoBehaviour
{
    // Private reference variable
    private GameObject _toxicUI;

    private void Start()
    {
        // Get the reference to the toxic image
        _toxicUI = GameObject.Find("ToxicUI");
        // Hide the toxic image
        _toxicUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the toxic area
        if(other.CompareTag("Player"))
        {
            PlayerInfo.isPoisoned = true;
            // Display the toxic image
            _toxicUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player left the toxic area
        if (other.CompareTag("Player"))
        {
            // Indicate the end of the increase posion effect
            PlayerInfo.isPoisoned = false;
            // Indicate that the player is under the posion effect.
            PlayerInfo.isUnderPoisonEffect = true; // Should be deactivited by the PlayerPoisonController

            // Hide the toxic image
            _toxicUI.SetActive(false);
        }
    }

}
