using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Day Settings")]
    public Light directionalLight;
    [Range(1, 100)]
    public int dayLongInMinutes = 10;
    [Range(1, 100)]
    public int nightLongInMinutes = 10;


    // Private value variables
    private int _HOURS_PER_DAY = 24;
    private int _SECONDS_PER_MINUTE = 60;
    private int _MINUTES_PER_HOUR = 60;
    private float _timer;
    private bool _isNight;
    private float _sleepAnimationTimer = -1;
    private float _timeSpeedWhileSleeping;

    // Start is called before the first frame update
    void Awake()
    {
        // Reset the time
        _timer = dayLongInMinutes * _SECONDS_PER_MINUTE;
        _isNight = false;
    }

    private void Start()
    {
        startANewDay();
    }

    // Update is called once per frame
    void Update()
    {
        if(_timer > 0)
        {
            if(!PlayerInfo.isSleeping)
                _timer -= Time.deltaTime; // Normal time speed
            else
            {// The player is sleeping

                // Check if the speed start time was indicated
                if(_sleepAnimationTimer < 0)
                {// The speed start time was not registered
                    // Start the sleep speed up animation timer
                    _sleepAnimationTimer = PlayerInfo.SLEEP_ANIMATION_TIME;

                    // Calculate the day time in seconds(in game time)
                    int dayLongInSeconds = (nightLongInMinutes + dayLongInMinutes) * _SECONDS_PER_MINUTE;

                    // Calculate game second per world hour
                    float secondsPerHour = dayLongInSeconds / _HOURS_PER_DAY;

                    // Calculate how many seconds need to pass before the sleep time is over
                    float secondsToWakeUp = secondsPerHour * PlayerInfo.SLEEP_TIME;

                    // Calculate the seconds in speed up mood per second in normal mood
                    _timeSpeedWhileSleeping = secondsToWakeUp / PlayerInfo.SLEEP_ANIMATION_TIME;
                }

                // Increase the time speed so the player sleep hours ends
                _timer -= _timeSpeedWhileSleeping * Time.deltaTime;
                // Count down to the end of the sleep animation
                _sleepAnimationTimer -= Time.deltaTime;
            }
            updateLight(_isNight ? (1 - _timer / (nightLongInMinutes * _SECONDS_PER_MINUTE))
                        : _timer / (dayLongInMinutes * _SECONDS_PER_MINUTE));

            // Check if the sleep animation ended
            if(PlayerInfo.isSleeping && _sleepAnimationTimer < 0)
            {// The sleep time is over
                PlayerInfo.isStopped = false;
                PlayerInfo.isSleeping = false;

                // Display the information
                PlayerInterfaceController.displayInfo();
            }
        }
        else
        {
            if(!_isNight)
            {// It was day and the day ended
                _isNight = true;
                // The minus _timer part is to ensure the speed when player sleeping fits with the animation time
                _timer = nightLongInMinutes * _SECONDS_PER_MINUTE - Mathf.Abs(_timer);
                // Count the day
                PlayerInfo.currentDay++;
                // Display the new info
                PlayerInterfaceController.displayInfo();
            }
            else
            {// It was a night and the sunrised now
                _isNight = false;
                // The minus _timer part is to ensure the speed when player sleeping fits with the animation time
                _timer = dayLongInMinutes * _SECONDS_PER_MINUTE - Mathf.Abs(_timer);
                startANewDay();
            }
        }
    }


    private void updateLight(float timePercent)
    {
        if (timePercent < 0)
            timePercent = 0;
        //byte grayScaleDayTime = (byte)(timePercent * byte.MaxValue);
        //Debug.Log(grayScaleDayTime);
        //directionalLight.color = new Color(grayScaleDayTime, grayScaleDayTime, grayScaleDayTime);
        directionalLight.color = new Color(timePercent, timePercent, timePercent);
    }

    private void startANewDay()
    {
        // Should call the fillGarbageLocations method
        GetComponent<FillGarbageLocations>().fillGarbageLocations();


        // Should rise the sun

        // Should wake the Waste Pickers(NPCs)
    }


    private void startANewNight()
    {
        // Should set the sun

        // Should send the Waste Pickers to sleep
    }

    /// <summary>
    /// Calculate the time in 24h-rethem
    /// </summary>
    /// <returns>The current hour in range 0-24</returns>
    public float currentTime()
    {
        float currentHour = 0;

        // if it is day add the night hours
        if (!_isNight)
        {
            // Calculate the current time, so the count start from zero
            currentHour = dayLongInMinutes * _SECONDS_PER_MINUTE - _timer + nightLongInMinutes * _SECONDS_PER_MINUTE;
        }
        else
        {// It is night time
            // Calculate the current time, so the count start from zero
            currentHour = nightLongInMinutes * _SECONDS_PER_MINUTE - _timer;
        }
        

        // Calculate the current hour in percent 0f-1f
        currentHour /= (dayLongInMinutes + nightLongInMinutes) * _SECONDS_PER_MINUTE;

        // Calculate the current hour in world time
        currentHour *= _HOURS_PER_DAY;


        // Avoid returning a negative number
        return currentHour < 0 ? 0 : currentHour;
    }

    public float convertGameSecondToDayMinutes(float gameTimeInSecond)
    {
        // Calculate the day time in seconds(in game time)
        int dayLongInSeconds = (nightLongInMinutes + dayLongInMinutes) * _SECONDS_PER_MINUTE;

        // Calculate game second per world hour
        float secondsPerHour = dayLongInSeconds / _HOURS_PER_DAY;

        // Return how many hour should have passed from a real 24h day
        return (gameTimeInSecond / secondsPerHour) * _MINUTES_PER_HOUR;
    }

    /// <summary>
    /// Calculate the seconds that equal to game hours(hours parameter)
    /// </summary>
    /// <param name="hours">The game hours to be calculated to world seconds.</param>
    /// <returns>The real world seconds that is equal to the game hours.</returns>
    public float convertDayHoursToGameSeconds(float hours)
    {
        // Calculate how long is the game day in real world second
        float gameDayLongInSecond = (dayLongInMinutes + nightLongInMinutes) * _SECONDS_PER_MINUTE;

        // Calculate the seconds in a normal world day
        float worldDayLongInSecond = _HOURS_PER_DAY * _MINUTES_PER_HOUR * _SECONDS_PER_MINUTE;

        // Calculate the time to shift in real world second
        float timeToShiftInSecond = (hours * _MINUTES_PER_HOUR * _SECONDS_PER_MINUTE);

        // Return the division between the world day second and the game day second
        return ((gameDayLongInSecond / worldDayLongInSecond) * timeToShiftInSecond);
    }

    public void shiftTime(float timeToShiftInHours)
    {
        // Calculate the seconds that need to pass
        float timeToShiftInGameSeconds = convertDayHoursToGameSeconds(timeToShiftInHours);

        // Decrease the time
        _timer -= timeToShiftInGameSeconds;

    }

}
