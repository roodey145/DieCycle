using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsMenuController : MonoBehaviour
{
    // Reference variables
    public GameObject parentObject;
    private AudioManager _audioManager;

    // Private value variables
    private string _cantEatFoodSound = "CanNotEatMoreFood";
    private string _noMoneySound = "NoMoney";

    private void Start()
    {
        // Get the reference to the audio manager
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void buyAnItem(ItemInfo itemInfo)
    {
        // Check if the player has enough money
        if(PlayerInfo.money > itemInfo.price)
        {// The player has enough money to buy this item

            // Check if the player energy is full
            if(PlayerInfo.energy < PlayerInfo.MAX_ENERGY)
            {// The player needs energy
                // Decrease the player money
                PlayerInfo.money -= itemInfo.price;

                // Check if the player energy will exceed the max allowed amount
                if(PlayerInfo.energy + itemInfo.energy > PlayerInfo.MAX_ENERGY)
                {// Taking this food will increase the player energy to exceed the allowed amount
                    // Set the energy to max
                    PlayerInfo.energy = PlayerInfo.MAX_ENERGY;
                }
                else
                {// The player energy is still below the max after taking this food
                    // Increase the player energy
                    PlayerInfo.energy += itemInfo.energy;
                }
                // Display the new information
                PlayerInterfaceController.displayInfo();
            }
            else
            {
                // Play the not hungry/can't eat sound
                _audioManager.Play(_cantEatFoodSound);
            }
            

           
        }
        else
        {// The player has not enough money to buy the item
         // Play the has no enough money sound
            _audioManager.Play(_noMoneySound);
        }

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;


        // Hide the menu
        parentObject.SetActive(false);
        PlayerInfo.isStopped = false;
        ItemSeller.isMenuOpen = false; // Indicate that the menu is closed
    }
}
