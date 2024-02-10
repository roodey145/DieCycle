using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [Header("Inputs")]
    public TMP_InputField usernameTextField;
    public TMP_InputField passwordTextField;


    private static string path = "Users/";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void login()
    {
        string username = usernameTextField.text;
        string password = passwordTextField.text;

        try
        {
            // Check if the user information exits
            if (!Directory.Exists(path + username))
                throw new FileNotFoundException("The user does not exits");

            // Get the user password
            StreamReader passwordReader = new StreamReader(path + username + "/" + username + ".txt");
            string realPassword = passwordReader.ReadLine();

            if (realPassword.Equals(password))
            {   // The user exits and the intested password matches

                //Debug.Log(password);

                // Get the user information
                StreamReader infoReader = new StreamReader(path + username + "/information.txt");
                string info = infoReader.ReadLine();

                // Extract the info [CurrentDay, ExhuastionLevel, Energy, Garabage, Money]
                string[] playerInfo = info.Split('-');

                // Check if the playerInfo contains the required information
                if (playerInfo.Length < 6)
                    throw new Exception("The player information is missing!");

                // Set the player info
                PlayerInfo.setInfo(
                    int.Parse(playerInfo[0])
                    , int.Parse(playerInfo[1])
                    , int.Parse(playerInfo[2])
                    , int.Parse(playerInfo[3])
                    , int.Parse(playerInfo[4])
                    , float.Parse(playerInfo[5]));

                // Open the game area
                SceneManager.LoadScene("GameArea");
            }
            else
            {   // The password is incorrect 
                throw new Exception("The intested password does not match! " + realPassword);
            }


        }
        catch (FileNotFoundException fileNotFound)
        {
            Debug.LogError("This user name does not exists!" + username + " : " + password);
        }
        catch (Exception fileIsEmpty)
        {
            Debug.LogError(fileIsEmpty);
        }
    }
}
