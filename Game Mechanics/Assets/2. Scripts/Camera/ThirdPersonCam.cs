using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    private Transform orientation;
    private Transform player;
    private Transform playerObj;
    private Rigidbody playerRb;
    private CharacterController playerController;

    [HideInInspector] public Vector3 viewDirection;
    [HideInInspector] public Vector3 inputDirection;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    private float rotationSpeed;


    void Start()
    {
        // Finding player components
        player = FindObjectOfType<CharacterController>().transform;

        orientation = player.GetChild(0).transform;
        playerObj = player.GetChild(1).transform;
        playerRb = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<CharacterController>();

        rotationSpeed = playerController.cameraRotationSpeed;

        // Setting Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {
        // Rotate orientation
        viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        // Rotate Player object
        horizontalInput = playerController.horizontalInput;
        verticalInput = playerController.verticalInput;
        inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
