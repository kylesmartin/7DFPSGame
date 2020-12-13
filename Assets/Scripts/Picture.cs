using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
    // Public variables
    public int id;
    public KeyObject keyObject;
    public GameObject background;

    // Private variables

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void InitPicture(int _id, KeyObject _keyObject, GameObject _background)
    {
        id = _id;
        keyObject = _keyObject;
        background = _background;
    }
}
