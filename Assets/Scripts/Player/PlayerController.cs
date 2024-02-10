using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    public static float mouseSensitivity = 1;
    [Range(100, 1000)]
    public float mouseRotationSpeed = 100;
    [Range(30, 90)]
    public int maxAngle = 60;



    // Private reference variables
    private CharacterController _controller;
    private GameObject _camera;

    // Private value variables
    private float _mouseSensitivity = 100;
    [SerializeField] private float movementSpeed = 5;
    public float decreasSpeed = 0;
    private float _currentAngle;
    private float _initialYAkse;



    private void Awake()
    {

        // Hide the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the required references
        _controller = GetComponent<CharacterController>();
        _camera = transform.Find("MainCamera").gameObject;

        // Get the y akse of the player to lock it
        _initialYAkse = transform.position.y;

        // Calculate the mouse speed
        _mouseSensitivity = mouseRotationSpeed * mouseSensitivity;
    }

    private void FixedUpdate()
    {
        // Calculate the mouse speed
        _mouseSensitivity = mouseRotationSpeed * mouseSensitivity;

        if (!PlayerInfo.isStopped)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        // Decides which direction to move based on what the player input's.
        Vector3 direction = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // Calculates what direction the charater is supposed to move to based on the rotation of the character. 
        Vector3 targetDirection = transform.forward * direction.z + transform.right * direction.x;

        // Lock the player y position
        transform.localPosition = new Vector3(transform.localPosition.x, _initialYAkse, transform.localPosition.z);
        _controller.enabled = false;
        _controller.transform.localPosition = transform.localPosition;
        _controller.enabled = true;

        // Moves the character using the character controller component
        _controller.Move(targetDirection * (movementSpeed - decreasSpeed) * Time.deltaTime);
    }

    private void Rotate()
    {
        // Rotates the character based on the mouse's movement on the X-axis
        transform.Rotate(Vector3.up, Time.deltaTime * Input.GetAxis("Mouse X") * _mouseSensitivity);

        // Rotate the camera up and down based on the mouse's movement on the Y-axis
        float z = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _currentAngle -= z;
        _currentAngle = Mathf.Clamp(_currentAngle, -maxAngle, maxAngle);
        _camera.transform.eulerAngles = 
            new Vector3(_currentAngle, _camera.transform.eulerAngles.y, _camera.transform.eulerAngles.z);
    }
}
