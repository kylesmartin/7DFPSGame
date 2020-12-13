using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    // Public variables
    public float maxFieldOfView = 100f;
    public float minFieldOfView = 40f;
    public float fieldOfViewStep = 2f;

    // Private variables
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            if (PlayerMovement.instance.isGrounded)
            {
                animator.SetBool("isWalking", PlayerMovement.instance.x != 0 || PlayerMovement.instance.z != 0);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            bool _isAiming = Input.GetMouseButton(1);
            animator.SetBool("isAiming", _isAiming);
            GameManager.instance.isAiming = _isAiming;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && GameManager.instance.cam.fieldOfView < maxFieldOfView)
            {
                GameManager.instance.cam.fieldOfView += fieldOfViewStep;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && GameManager.instance.cam.fieldOfView > minFieldOfView)
            {
                GameManager.instance.cam.fieldOfView -= fieldOfViewStep;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAiming", false);
            GameManager.instance.isAiming = false;
        }
    }
}