using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoisonController : MonoBehaviour
{
    [SerializeField]
    [Range(0.05f, 0.5f)]
    private float _toxicCleanserSpeed = 0.25f;
    private float _palyerSpeedDecrease = 4;
    private float _timeSpentInToxicArea;
    private float _lastToxicTimePuchment;
    private bool _gotToxicPunchment; // To indicate the lose of energy after entering a toxic area

    // Private reference variables
    private PlayerController _playerController; 
    private TimeController _timeController;
    private AudioManager _audioManager;


    // Private value variables
    private string _painSound = "Pain";


    // Start is called before the first frame update
    void Start()
    {
        // Get reference to the time controller
        _timeController = GameObject.Find("Controller").GetComponent<TimeController>();

        // Get reference to the player controller to decrease the speed when poisoned
        _playerController = GetComponent<PlayerController>();

        // Get the reference to the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is poisoned
        if(PlayerInfo.isPoisoned) // Poison is activited by the ToxicArea variable
        {
            // Indicates that the player needs to receive a puchment after leaving the toxic area
            _gotToxicPunchment = false;

            // Will be used to calculate the time needed before the player die
            _timeSpentInToxicArea += Time.deltaTime;

            // Increase the toxic level
            PlayerInfo.toxic += 
                _timeController.convertGameSecondToDayMinutes(Time.deltaTime) * PlayerInfo.TOXIC_INCREASE_PER_MINUTE;

            // Check if the player exceeded the allowed toxic amount
            if(PlayerInfo.toxic >= PlayerInfo.MAX_TOXIC)
            {// The player dead because of toxic
                PlayerInfo.isDead = true;


                // Set the player toxic to max
                PlayerInfo.toxic = PlayerInfo.MAX_TOXIC;
            }

            // Update the user information
            PlayerInterfaceController.displayInfo();
            
        }
        // Check if the player was poisoned
        else if(PlayerInfo.isUnderPoisonEffect)
        {
            // Check if the punchment for entering the toxic area was received
            if(!_gotToxicPunchment)
            {
                // Decrease the player speed
                _playerController.decreasSpeed = _palyerSpeedDecrease;

                // Calculate the time spent in the toxic area
                /*
                 * If the player spent 1 sec in the toxic area then leave it
                 * the player will receive puchment for this second. 
                 * If the player enter immeditly again and wait another second
                 * the player should not receive the puchment for 2 seconds. 
                 * The problem is that the toxic effect will be reduced gradually
                 * after the player leaves the toxic area, that means the variable
                 * _timeSpentInToxicArea will not become 0 immeditly, therefor 
                 * another variable was made to remember the time in which the
                 * punchment was received for. By subtracting those two values
                 * the delta (LastTimeSpentInToxic) can be calculated
                 */
                float deltaTime = _timeSpentInToxicArea - _lastToxicTimePuchment;

                // Calculate the amount of energy the body needs to resist the poison
                float energyToLose = 
                    _timeController.convertGameSecondToDayMinutes(deltaTime)
                    * PlayerInfo.TOXIC_PUNCHMENT_PER_MINUTE;

                // Play the toxic pain audio
                _audioManager.Play(_painSound);

                // Check if the energy lose is greater than the player energy
                if(Mathf.FloorToInt(energyToLose) > PlayerInfo.energy)
                {
                    // Die
                    PlayerInfo.energy = 0;

                    PlayerInfo.isDead = true;
                }
                else
                {
                    PlayerInfo.energy -= Mathf.FloorToInt(energyToLose);
                }

                // Assign the _timeSpentInToxicArea to the _lastToxicTimePuchment
                _lastToxicTimePuchment = _timeSpentInToxicArea;

                // Display the player new info
                PlayerInterfaceController.displayInfo();

                _gotToxicPunchment = true;
            }
            // Decrease the time(Clean the body from the poison)
            _timeSpentInToxicArea -= Time.deltaTime * _toxicCleanserSpeed;

            // Decrease the last time received for the puchment
            _lastToxicTimePuchment -= Time.deltaTime * _toxicCleanserSpeed;

            // Decrease the toxic 
            PlayerInfo.toxic -=
                _timeController.convertGameSecondToDayMinutes(Time.deltaTime * _toxicCleanserSpeed)
                * PlayerInfo.TOXIC_INCREASE_PER_MINUTE;

            // Update the user information
            PlayerInterfaceController.displayInfo();

            // Check if the body is cleansed from the toxic
            if (_timeSpentInToxicArea <= 0)
            {
                // Return the player speed to normal
                _playerController.decreasSpeed = 0;

                // Decrease the last time received for the puchment
                _lastToxicTimePuchment = 0;

                // Indicate that the toxic is removed
                PlayerInfo.isUnderPoisonEffect = false;
            }
        }
    }
}
