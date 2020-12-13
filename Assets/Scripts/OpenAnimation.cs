using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAnimation : MonoBehaviour
{
    // Public variables
    public GameObject key;

    // Private variables
    private Animator animator;
    private bool isOpen;
    private bool isLocked;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        isLocked = false;
        if (key != null) isLocked = true;
    }

    // Update is called once per frame
    void Update() 
    {
        for (int i = 0; i < GameManager.instance.inventory.Count; i++)
        {
            if (GameManager.instance.inventory[i] == key)
            {
                isLocked = false;
            }
        }
    }

    public void FlipOpenState()
    {
        if (isLocked == false)
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
        } else
        {
            Debug.Log("door is locked. find the key.");
        }
    }
}
