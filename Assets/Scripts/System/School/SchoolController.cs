using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolController : MonoBehaviour
{
    [Header("School Settings")]
    public int lecturesTimeStart = 8;
    public int lecturesTimeEnd = 14;
    [Range(0.01f, 0.05f)]
    public float knowledge;

    [Header("Notification Settings")]
    public string schoolOpenNotification = "The School Is Open";
    public Color notificationColor = Color.green;


    // Private reference variables
    private TimeController _timeController;
    private AudioManager _audioManager;

    // Private variables
    private string _schoolIsClosedAudio = "SchoolIsClosed";
    private bool _notificationSent = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the time controller
        _timeController = GameObject.Find("Controller").GetComponent<TimeController>();

        // Get the reference to the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current time
        float currentTime = _timeController.currentTime();

        // Check if the school is open
        if (currentTime < lecturesTimeEnd && currentTime > lecturesTimeStart)
        {
            if(!_notificationSent)
            {
                // Send the message to the player
                MessageController.message = schoolOpenNotification;

                // Set the notifaction duration
                MessageController.messageDuration = _timeController.convertDayHoursToGameSeconds(lecturesTimeEnd - currentTime);

                // Set the notification color
                MessageController.messageColor = notificationColor;

                // Mark that the notification was sent
                _notificationSent = true;
            }
        }
        else if(_notificationSent)
        {
            // Check if the notification is still displayed
            if(MessageController.message.Equals(schoolOpenNotification))
            {// A time shfit happend. Reset the notification duration
                MessageController.messageDuration = 0;
                _notificationSent = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {// The player is trying to enter the school
            // Get the current time
            float currentTime = _timeController.currentTime();

            // Check if the school is open
            if(currentTime < lecturesTimeEnd && currentTime > lecturesTimeStart)
            {// The school is open
                // Calculate the player energy to be used in school
                int energyToBeUsed = (int)((lecturesTimeEnd - currentTime) * PlayerInfo.EXHUSTION_PER_HOUR);

                // Decrease the player energy
                PlayerInfo.energy -= energyToBeUsed;

                // Check if the player energy us below 0
                if(PlayerInfo.energy < 0)
                {// The player is dead(lost all the energy)

                    // Indicate that the player is dead
                    PlayerInfo.isDead = true;
                }
                else
                {
                    //Debug.Log(currentTime);
                    // The player is not dead --> shift the time
                    _timeController.shiftTime(lecturesTimeEnd - currentTime);

                    // Decrease the garbage pick animation delay
                    Garbage.garbageAnimationTime -= knowledge;
                    //Debug.Log(_timeController.currentTime());
                }

                // Display the new information
                PlayerInterfaceController.displayInfo();
            }
            else
            {
                // Play the school is closed sound
                _audioManager.Play(_schoolIsClosedAudio);
                //Debug.Log("School closed");
            }
        }
    }
}
