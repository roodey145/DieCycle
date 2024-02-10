using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHome : MonoBehaviour
{
    [Header("Sleep Settings")]
    [Range(15f, 23f)]
    public float sleepTimeAfter = 17;
    [Range(0f, 5f)]
    public float sleepTimeBefore = 4;


    [Header("Sleep Time Notification")]
    public string sleepTimeNotification = "Sleep Time";
    public Color notificationColor = Color.red;

    private TimeController _timeController;

    private bool _notificationSent = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the time controller component from the controller object
        _timeController = GameObject.Find("Controller").GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerInfo.isSleeping)
        {
            // Get the current time
            float currentTime = _timeController.currentTime();

            // Check if the school is open
            if (currentTime < sleepTimeBefore || currentTime > sleepTimeAfter)
            {
                if (!_notificationSent)
                {
                    // Send the message to the player
                    MessageController.message = sleepTimeNotification;

                    float notificationDuration = 0;

                    // Check if the current time is after the sleep time and before midtnight
                    if(currentTime > sleepTimeAfter)
                    {
                        // Calculate the notification duration considering the time is before midtnight
                        notificationDuration = (sleepTimeAfter + sleepTimeBefore) - currentTime;
                    }
                    else
                    {// The current time is after midnight which means it can be between zero and sleepTimeBefore

                        // Calculate the notification duration
                        notificationDuration = sleepTimeAfter - currentTime;
                    }

                    // Set the notifaction duration
                    MessageController.messageDuration = _timeController.convertDayHoursToGameSeconds(notificationDuration);

                    // Set the notification color
                    MessageController.messageColor = notificationColor;

                    // Mark that the notification was sent
                    _notificationSent = true;
                }
            }
            else if (_notificationSent)
            {
                // Check if the notification is still displayed
                if (MessageController.message.Equals(sleepTimeNotification))
                {// A time shfit happend. Reset the notification duration
                    MessageController.messageDuration = 0;
                    _notificationSent = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // Check if the player entered the door range
        if (other.CompareTag("Player"))
        {
            // Get the current time
            float currentTime = _timeController.currentTime();

            // Debug.Log(currentTime);

            // Check this is the sleep time / night time
            if(currentTime > sleepTimeAfter || currentTime < sleepTimeBefore)
            {
                PlayerInfo.isStopped = true; // To prohibt the player movement
                playerSleep();
            }
        }
    }


    private void playerSleep()
    {
        PlayerInfo.isSleeping = true; // Indicates that the player is sleeping 

        // Increase the player energy
        // Calculate the energy gained from sleep time
        int increaseInEnergy = (int)(PlayerInfo.SLEEP_TIME * PlayerInfo.ENERGY_PER_SLEEP_HOUR);

        // Calculate the new energy 
        int newEnergy = increaseInEnergy + PlayerInfo.energy;

        // Check if the newEnergy exceed the max energy
        if (newEnergy > PlayerInfo.MAX_ENERGY)
            newEnergy = PlayerInfo.MAX_ENERGY;

        // Register the new energy level
        PlayerInfo.energy = newEnergy;

        // Save the progress
        PlayerInfo.registerCurrentProgress();
    }
}
