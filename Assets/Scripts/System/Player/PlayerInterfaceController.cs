using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInterfaceController : MonoBehaviour
{
    // Private reference to the text interfaces
    private static TextMeshProUGUI _garbageCountUI;
    private static TextMeshProUGUI _moneyCountUI;
    private static TextMeshProUGUI _dayCountUI;
    private static TextMeshProUGUI _energyLevelUI; // Should be replaced with a slider
    private static TextMeshProUGUI _toxicLevelUI; // Should be replaced with a slider
    private static Slider _energyBar;
    private static Slider _toxicBar;

    // Private value variables
    private static string _garbageCountMessage = "";
    private static string _moneyCountMessage = "";
    private static string _energyLevelMessage = "";

    private void Awake()
    {
        // Get the references to the required objects
        _garbageCountUI = GameObject.Find("GarbageCounter").GetComponent<TextMeshProUGUI>();
        _moneyCountUI = GameObject.Find("MoneyCounter").GetComponent<TextMeshProUGUI>();
        _dayCountUI = GameObject.Find("DayCounter").GetComponent<TextMeshProUGUI>();
        _energyLevelUI = GameObject.Find("Energy").GetComponent<TextMeshProUGUI>();
        _energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        _toxicLevelUI = GameObject.Find("Toxic").GetComponent<TextMeshProUGUI>();
        _toxicBar = GameObject.Find("ToxicBar").GetComponent<Slider>();

        // Set the max value of the energy bar slider
        _energyBar.maxValue = PlayerInfo.MAX_ENERGY;
        // Set the max value of the toxic bar slider
        _toxicBar.maxValue = PlayerInfo.MAX_TOXIC;
    }

    private void Start()
    {
        // Display the information
        displayInfo();
    }

    /// <summary>
    /// Updates the user UI(Energy, Toxic, Day, Money, and Garbage count).
    /// </summary>
    public static void displayInfo()
    {
        // Check if this is the toturial
        if (SceneManager.GetActiveScene().name.Equals("Tutorial"))
            return; // This is the tutorial
        _garbageCountUI.text = PlayerInfo.garbageCount.ToString();
        _moneyCountUI.text = string.Format("{0:0.00}", PlayerInfo.money);
        _dayCountUI.text = PlayerInfo.currentDay.ToString();

        // Update the energy value and slider slide leve
        _energyLevelUI.text = _energyLevelMessage + PlayerInfo.energy;
        _energyBar.value = PlayerInfo.energy;

        // Update the energy value and slider slide leve
        _toxicLevelUI.text = string.Format("{0:0.0}", PlayerInfo.toxic);
        _toxicBar.value = PlayerInfo.toxic;
    }
}
