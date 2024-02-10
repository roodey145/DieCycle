using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    


    // Priivate reference variables
    private TimeController _timeController;
    private AudioManager _audioManager;
    private Image _deathNotification;

    // Private value variables
    private float _currentHour;
    private int _collectedGarbage = 0;
    private bool _wasSleeping = false;

    // Start is called before the first frame update
    void Start()
    { 
        // Find the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();


        // Find the death notification and deactivite it 
        //deathNotification = GameObject.Find("DeathNotification").GetComponent<Image>();
        //deathNotification.gameObject.SetActive(false);



        // Get energy text interface
        //_energyTMP = GameObject.Find("Energy").GetComponent<TextMeshProUGUI>();

        // Get the time controller
        _timeController = GameObject.Find("Controller").GetComponent<TimeController>();

        // Get the current time
        _currentHour = _timeController.currentTime();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!PlayerInfo.isSleeping)
        { // The player is still alive

            // Check the energy used to collect garbage
            exertionUsedCheck();

            // Check if an hour passed to decrease the energy
            hourlyCheck();

        }
        else if(PlayerInfo.isSleeping && !_wasSleeping)
        {// The player is sleeping
         /* When the player is sleeping the last registered time will be the time before
          * the palyer went to sleep. When the sleep time ends, the current time will be
          * greter than the old registered time, to avoid losing energy once the player 
          * wakes up, this bool will be used */
            _wasSleeping = true;
        }
    }

    private void hourlyCheck()
    {
        float currentHour = _timeController.currentTime();
        
        // Check if the user was sleeping
        if(_wasSleeping)
        {
            // Set the last registered time to the current ime
            _currentHour = currentHour;

            // Indicate that the registered time was not registered before the player went to sleep
            _wasSleeping = false;
        }

        // Check if an hour passed
        if (Mathf.Abs(currentHour - _currentHour) > 1)
        {
            PlayerInfo.energy -= PlayerInfo.EXHUSTION_PER_HOUR;
            // Set the previous _currentHour to be the new detected time
            _currentHour = currentHour;

            // Check if the player energy is below 0
            if (PlayerInfo.energy <= 0)
            {
                // Start the death animation
                PlayerInfo.isDead = true;
            }
            else
            {
                PlayerInterfaceController.displayInfo();
            }
        }
    }

    /// <summary>
    /// Check the exertion used to collect the material
    /// </summary>
    private void exertionUsedCheck()
    {
        // Check the difference in the garbage collection from the last check
        int garbageCollectionIncrease = PlayerInfo.garbageCount - _collectedGarbage;
        if(garbageCollectionIncrease >= PlayerInfo.GARBAGE_COLLECTION_AMOUNT_BEFORE_ENERGY_DROP)
        {
            // Decrease the player energy
            PlayerInfo.energy--;
            // Register the new collected garbage amount
            _collectedGarbage = PlayerInfo.garbageCount;
            // Display the energy info
            PlayerInterfaceController.displayInfo();
        }
    }

    

    //private void displayHealthInfo()
    //{
    //    _energyTMP.text = _energyMessage + PlayerInfo.energy;
    //}
}
