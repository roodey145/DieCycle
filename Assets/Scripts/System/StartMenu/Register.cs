using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;

public class Register : MonoBehaviour
{

    [Header("Inputs")]
    public TMP_InputField usernameTextField;
    public TMP_InputField passwordTextField;
    public TMP_InputField rPasswordTextField;

    [SerializeField] private StartMenuUIController controller;

    private string path = "Users/";

    // Password requirements
    private static int _CAPITALIZED_LETTER = 1; // Minimum
    private static int _LETTER_COUNT = 2; // Minimum
    private static int _NUMBERS = 3; // Minimum 
    private static int _PASSWORD_LENGTH = 8; // Minimum

    public void register()
    {
        string username = usernameTextField.text;
        string password = passwordTextField.text;
        string rPassword = rPasswordTextField.text;

        // Check if the password and repeat password matches
        if(!password.Equals(rPassword))
        {
            throw new Exception("The passwords do NOT Match!");
        }

        // Check if the password meets the requirements
        bool passwordIsValid = isValid(password);

        try
        {
            // Check if the username already exists
            if (Directory.Exists(path + username))
                throw new Exception("This user already exits, please try to login instead");

            // Create the folder to hold the user information
            Directory.CreateDirectory(path + username);

            // Create password file
            StreamWriter createPassword = new StreamWriter(path + username + "/" + username + ".txt");
            createPassword.WriteLine(password);
            createPassword.Close();

            // Create user information
            StreamWriter createInformation = new StreamWriter(path + username + "/" + "information.txt");
            // [CurrentDay, ExhuastionLevel, Energy, Garbage, Money]
            createInformation.WriteLine("1-0-100-0-0");
            createInformation.Close();

            ////////////////////////////
            // The user was registered
            ///////////////////////////
            // Clear the fields
            //usernametextfield.text = "";
            //passwordtextfield.text = "";
            //rpasswordtextfield.text = "";

            // Open the login menu
            controller.ActiveMenu("Login");

        }
        catch (Exception fileIsEmpty)
        {
            Debug.Log(fileIsEmpty);
        }
    }


    // Validate password
    private bool isValid(string password)
    {
        // Check the length of the password if it match the requirement 
        if (password.Length < _PASSWORD_LENGTH)
            return false;


        int letters = 0;
        int capLetters = 0;
        int numbers = 0;

        for (int i = 0; i < password.Length; i++)
        {
            // Extract one character
            char currentCharacter = password[i];

            if (Char.IsLetter(currentCharacter))
                letters++;
            if (Char.IsUpper(currentCharacter))
                capLetters++;
            if (Char.IsDigit(currentCharacter))
                numbers++;
        }

        return (letters >= _LETTER_COUNT)
                && (capLetters >= _CAPITALIZED_LETTER)
                && (numbers >= _NUMBERS);
    }
}
