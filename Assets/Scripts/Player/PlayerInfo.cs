using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is intended to contains static variables
/// to allow the other scripts access it freely if needed. 
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    public static int exhuastion = 0;
    public static int MAX_EXHASTION = 100;
    private static int _energy = 100;
    public static int MAX_ENERGY = 100;
    public static int energy { 
        set {
            if (value < 0) _energy = 0;
            else if (value > MAX_ENERGY) _energy = MAX_ENERGY;
            else _energy = value;
            }
        get { return _energy; }
    }
    public static int EXHUSTION_PER_HOUR = 10;
    private static float _toxic = 0;
    public static float toxic
    {
        set
        {
            if (value < 0) _toxic = 0;
            else if (value > MAX_TOXIC) _toxic = MAX_TOXIC;
            else _toxic = value;
        }
        get { return _toxic; }
    }
    public static int MAX_TOXIC = 100;
    public static int TOXIC_INCREASE_PER_MINUTE = 2;
    public static float TOXIC_PUNCHMENT_PER_MINUTE = 1;
    public static int ENERGY_PER_SLEEP_HOUR = 10;
    // Means when the player collect 10 garbage pieces the energy will decrease by one
    public static int GARBAGE_COLLECTION_AMOUNT_BEFORE_ENERGY_DROP = 10; 
    public static int garbageCount = 0;
    public static int currentDay = 1;
    public static float money = 0; // Somali Shilling
    public static float CONVERT_TO_DKK = 88.72f;
    public static float CONVERT_TO_USD = 576.54f;
    public static float SLEEP_TIME = 8; // The sleep time in hour
    public static float SLEEP_ANIMATION_TIME = 3; // Will be used to accelerate the day rhythm

    // Player state
    public static bool isDead = false;
    public static bool isFrozen = false;
    public static bool isPoisoned = false;
    public static bool isUnderPoisonEffect = false;
    public static bool isStopped = false; // To prohibit the user from controlling the character
    public static bool isSleeping = false;


    // Private reference
    private static ProgressInfo _currentDay = new ProgressInfo();
    private static ProgressInfo _lastDay = new ProgressInfo();


    public static void setInfo(int currentDay, int exhuastion, int energy, int garbageCount, int money, float toxic)
    {
        PlayerInfo.exhuastion = exhuastion;
        PlayerInfo.energy = energy;
        PlayerInfo.garbageCount = garbageCount;
        PlayerInfo.currentDay = currentDay;
        PlayerInfo.money = money;
        PlayerInfo.toxic = toxic;
    }


    // To add the information of a new collected garbage
    public static void registerNewCollectedGarbage(float price)
    {
        garbageCount++;
        money += price;
    }


    public static void registerCurrentProgress()
    {
        _lastDay = _currentDay;
        _currentDay = new ProgressInfo(
              exhuastion
            , energy
            , garbageCount
            , currentDay + 1
            , money
            , toxic
            , isDead
            , isFrozen
            , isPoisoned
            , isUnderPoisonEffect);
    }


    public static void reset(RestartActionOnDeath action)
    {
        switch (action)
        {
            case RestartActionOnDeath.RESTART_FROM_CURRENT_DAY:
                // Reload the progress registered this morning
                _currentDay.reloadInfo();
                break;

            case RestartActionOnDeath.RESTART_FROM_LAST_DAY:
                // Reload the progress registered last day
                _lastDay.reloadInfo();
                break;

            case RestartActionOnDeath.RESTART_FROM_START:
                // Completly reset the info
                exhuastion = 0;
                energy = 100;
                garbageCount = 0;
                currentDay = 1;
                money = 0;

                isDead = false;
                isFrozen = false;
                isPoisoned = false;
                break;
        }

        // In case the player dead while collecting items
        isSleeping = false;
        isStopped = false;

        // Restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


/// <summary>
/// This class holds the progress for the last day. 
/// Those info will be used in case the player dead. 
/// </summary>
class ProgressInfo
{
    int exhuastion = 0;
    int energy = 100;
    int garbageCount = 0;
    int currentDay = 1;
    float money = 0; // Somali Shilling
    float toxic = 0;

    // Player state
    bool isDead = false;
    bool isFrozen = false;
    bool isPoisoned = false;
    bool isUnderPoisonEffect = false;

    public ProgressInfo()
    {

    }

    public ProgressInfo(
          int exhuastion
        , int energy
        , int garbageCount
        , int currentDay
        , float money
        , float toxic
        , bool isDead
        , bool isFrozen
        , bool isPoisoned
        , bool isUnderPoisonEffect)
    {
        this.exhuastion = exhuastion;
        this.energy = energy;
        this.garbageCount = garbageCount;
        this.currentDay = currentDay;
        this.money = money;
        this.toxic = toxic;
        this.isDead = isDead;
        this.isFrozen = isFrozen;
        this.isPoisoned = isPoisoned;
        this.isUnderPoisonEffect = isUnderPoisonEffect;
    }

    public void reloadInfo()
    {
        PlayerInfo.exhuastion = exhuastion;
        PlayerInfo.energy = energy;
        PlayerInfo.garbageCount = garbageCount;
        PlayerInfo.currentDay = currentDay;
        PlayerInfo.money = money;
        PlayerInfo.toxic = toxic;
        PlayerInfo.isDead = isDead;
        PlayerInfo.isFrozen = isFrozen;
        PlayerInfo.isPoisoned = isPoisoned;
        PlayerInfo.isUnderPoisonEffect = isUnderPoisonEffect;
    }
}