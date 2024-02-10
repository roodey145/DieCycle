using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Garbage : MonoBehaviour
{
    [Header("Garbage Info")]
    public string soundName = "Bottles";
    [Range(3f, 30f)]
    public float price = 0.2f;
    // Pickrate between 1 and 100
    [Range(1, 100)]
    public int pickRate = 50;
    [Range(1, 100)]
    public int dropRate = 50;

    // Private variable
    private bool _isCollecting = false;
    private float _animationTimer;
    private Vector3 _defaultScale;
    private AudioManager _audioManager;

    // Static variables 
    private static float MIN_ANIMATION_DELAY = 0.1f;
    private static float _garbageAnimationTime = 0.3f;
    public static float garbageAnimationTime
    {
        set
        {
            // Prevent the animation delay to go below the minimum
            if (value < MIN_ANIMATION_DELAY) _garbageAnimationTime = MIN_ANIMATION_DELAY;
            else _garbageAnimationTime = MIN_ANIMATION_DELAY;
        }
        get
        {
            return _garbageAnimationTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the animation time
        _animationTimer = garbageAnimationTime;
        // Remeber the default scale to use it for the animation
        _defaultScale = transform.localScale;

        // Get the audio manager controller
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Detect if the player is collecting this object
        if (_isCollecting)
        {// The player is collecting this object

            // Animate the scale of the object depending on the animation time
            _animationTimer -= Time.deltaTime;

            // Check if the animation is over
            if (_animationTimer >= 0)
            {
                // Caclulate the new scale of the garbage depends on the passed time of the animation
                float scaleDown = _animationTimer / garbageAnimationTime;
                // Scale the garbage down
                transform.localScale = _defaultScale * scaleDown;
            }
            else
            {// The animation ended
                _isCollecting = false;

                // Allow the player movement again
                PlayerInfo.isStopped = false;

                // Check if this is a normal garbage
                if(gameObject.CompareTag("Garbage"))
                {
                    // Register the new collected garbage information
                    PlayerInfo.registerNewCollectedGarbage(price);


                    // Display the new information
                    PlayerInterfaceController.displayInfo();

                }
                else
                {// A toxic garbage
                    // Get access to the player job manager and register the progress
                    GameObject.FindGameObjectWithTag("Player").GetComponent<JobManager>().registerProgress();
                }
                
                // Destroy the garbage
                Destroy(gameObject, 0);
            }
        }
    }


    public void collect()
    {
        // Play the collecting sound 
        _audioManager.Play(soundName);

        //_audioManager.Play(_audioName);
        _isCollecting = true;

        // Prohibt the player from controlling the character 
        PlayerInfo.isStopped = true; // Stopped until the end of the animation
    }

    public void focus(float extraScale)
    {
        // Increase the size of this piece
        transform.localScale = _defaultScale + (_defaultScale * extraScale);
    }

    public void removeFocus()
    {
        // Return the piece to its original size
        transform.localScale = _defaultScale;
    }

    public bool isCollecting()
    {
        return _isCollecting;
    }
}
