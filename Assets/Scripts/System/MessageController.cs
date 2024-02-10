using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public static string message = "";
    public static float messageDuration = 0;
    public static Color messageColor;

    // Private references
    private TextMeshProUGUI _messageText;
    private TimeController _timeController;

    // Private value variables
    private string _clockMessage = " O'clock";
    private float _blinkSpeed = 2;
    private float _speedDirection = -1;
    private readonly float _MIN_ALPHA = 0.3f;
    private Color _defaultColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        // Get the text component
        _messageText = GetComponent<TextMeshProUGUI>();

        // Get the time controller
        _timeController = GameObject.Find("Controller").GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Chcek if there is a message to display
        if(messageDuration > 0 && messageColor != null)
        {
            // Check if the new message was added to the notification area
            if(!_messageText.text.Equals(message))
            {// The new message is not added yet

                // Add the message
                _messageText.text = message;
            }

            messageDuration -= Time.deltaTime;

            // // // Make the text blink
            // Get the text color 
            Color textColor = messageColor;
            textColor.a = _messageText.color.a;

            // Decrease/Increase the color alpha
            textColor.a += _blinkSpeed * _speedDirection * Time.deltaTime;

            // Check if the alpha is below the _MIN_ALPHA
            if (textColor.a < _MIN_ALPHA)
            {// The text alpha reached below the lowest alpha limit

                // Set the text alpha to the lowest alpha
                textColor.a = _MIN_ALPHA;

                // Reveres the effect and make the text appear again
                _speedDirection = 1;
            }
            else if(textColor.a > 1)
            {// The text has exceeded the max alpha

                // Set the text alpha to max alpha
                textColor.a = 1;

                // Reveres the effect and make the text disappear
                _speedDirection = -1;
            }

            // Apply the color to the message
            _messageText.color = textColor;

            if (messageDuration <= 0)
            {
                // Return the message alpha to its default
                _messageText.color = _defaultColor;

                messageDuration = 0;
            }
        }
        else
        {// There is no message to display

            // Check if the color has been reset
            if (!_messageText.color.Equals(_defaultColor))
            {
                // Reset the color
                _messageText.color = _defaultColor;
            }

            // Get the current time and added it as a message
            _messageText.text = ((int)_timeController.currentTime()).ToString() + _clockMessage;
        }

        

    }
}
