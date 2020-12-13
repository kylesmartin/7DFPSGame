using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Public variables 
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    public float rotationScale = 2f;

    // Private variables
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 

            float _mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
            float _mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

            xRotation -= _mouseY * rotationScale;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * _mouseX * rotationScale);
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
