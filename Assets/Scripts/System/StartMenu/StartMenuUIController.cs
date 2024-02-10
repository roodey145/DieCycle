using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUIController : MonoBehaviour
{
    [Header("Menus")]
    public GameObject[] menus;


    // Private reference variables
    private GameObject _startButton;
    private GameObject _exitButton;


    private string activeMenu;

    // Start is called before the first frame update
    void Start()
    {
        activeMenu = "Main";

        // Get the reference to the buttons
        _startButton = GameObject.Find("StartButton");
        _exitButton = GameObject.Find("ExitButton");
    }


    private void Update()
    {
        // // // Organize the start and exit buttons
        // Change the location of the start button
        _startButton.transform.position = new Vector2(
            Screen.width * 1.5f / 3,
            Screen.height * 1.85f / 3
            );

        // Change the location of the exit button
        _exitButton.transform.position = new Vector2(
            Screen.width * 1.5f / 3,
            Screen.height * 1.25f / 3
            );

        // Change the scale of the start button
        _startButton.GetComponent<RectTransform>().sizeDelta = new Vector2(
            Screen.width * 0.85f / 3,
            Screen.height * 1 / 10
            );


        // Change the scale of the exit button
        _exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(
            Screen.width * 0.85f / 3,
            Screen.height * 1 / 10
            );
    }


    /// <summary>
    /// Activite one of the menus and deactivite the rest
    /// </summary>
    /// <param name="menuName">The name of the menu to be opened</param>
    public void ActiveMenu(string menuName)
    {
        //GameObject menuToOpen = null;
        // Check if the menu exists
        for(int i = 0; i < menus.Length; i++)
        {
            // Check if this menu is the one to be activited
            if (menus[i].name == menuName)
            {
                // Activite the menu
                menus[i].SetActive(true);

                // Search for the old activited menu
                for (int ii = 0; ii < menus.Length; ii++)
                {
                    // Check if this menu is the last activited menu
                    if (menus[ii].name == activeMenu)
                    {
                        // Deactivite the old activited menu
                        menus[ii].SetActive(false);
                        // Remember the new menu as the activited menu
                        activeMenu = menuName;
                        break; // Stop searching
                    }
                }
                break; // Stop searching
            }
        }
    }


    public void start()
    {
        // Start the intro video
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
