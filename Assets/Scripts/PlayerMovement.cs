using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variables
    public static PlayerMovement instance;
    public float speed = 12f;
    public float gravity = -19.62f;
    public float groundCheckRadius = 0.4f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isGrounded;
    public float x;
    public float z;

    // Private variables
    private Vector3 velocity;
    private CharacterController characterController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
        if (!GameManager.instance.isPaused)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            Vector3 _move = x * transform.right + z * transform.forward;
            characterController.Move(_move * speed * Time.deltaTime);

            // if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
