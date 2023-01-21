using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CharacterController : MonoBehaviour, IDamagable
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    private Transform orientation;
    private Rigidbody playerRb;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    private Vector3 moveDirection;
    private Vector3 flatVelocity;
    private Vector3 limitedVelocity;

    [Header("Air Movement")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;


    [Header("Camera")]
    public float cameraRotationSpeed;


    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Stats")]
    public int health = 80;
    public int maxHealth = 80;
    public int coins = 0;

    [Header("UI")]
    public TMP_Text coinNumberTmp;
    


    void Start()
    {
        orientation = transform.GetChild(0).transform;
       
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;

        UpdateHealthBar();
    }

    private void Update()
    {
        GroundCheck();
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update functions
    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            playerRb.drag = groundDrag;
        }
        else
        {
            playerRb.drag = 0;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump behaviour
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void SpeedControl()
    {
        flatVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            limitedVelocity = flatVelocity.normalized * moveSpeed;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


    // Fixed Update functions
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded) // On ground
        {
            playerRb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded) // In air
        {
            playerRb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
           
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        UpdateHealthBar();

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        GameStateManager.Instance.LoseCondition(0);
    }

    public void UpdateHealthBar()
    {
        var healthBar = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();

        healthBar.fillAmount = (float)health / maxHealth;

        if (healthBar.fillAmount < 0.25f)
        {
            healthBar.color = Color.red;
        }
        else if (healthBar.fillAmount < 0.5f)
        {
            healthBar.color = Color.yellow;
        }
        else
        {
            healthBar.color = Color.green;
        }
    }

    public void UpdateCoinAmount(int coinAmount)
    {
        coins += coinAmount;
        coinNumberTmp.text = "Coins: " + coins;
    }
}
