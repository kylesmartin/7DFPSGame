using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    // Public variables
    public Text passwordText;

    // Private variables
    private string password;

    // Start is called before the first frame update
    void Start()
    {
        password = "";
    }

    // Update is called once per frame
    void Update()
    {
        passwordText.text = password;
        if (password == "alien1979")
        {
            Debug.Log("you got it");
        }
    }

    public void AddToPassword(string c)
    {
        password += c;
    }

    public void Backspace()
    {
        password.Remove(password.Length - 1);
    }
}
