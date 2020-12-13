using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public variables
    public static GameManager instance;
    public Camera mainCamera;
    public Camera cam;
    public ButtonAnimation cameraButton;
    public Transform[] worldSpaceCorners;
    public Transform objectives;
    public float minTimeBetweenPhotos = 1f;
    public bool isPaused;
    public bool isAiming;
    public List<KeyObject> keyObjectsFound;
    public List<GameObject> inventory;

    // Private variables
    private KeyObject[] keyObjects;
    private float timeSinceLastPhoto;

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
        keyObjectsFound = new List<KeyObject>();
        inventory = new List<GameObject>();
        keyObjects = FindObjectsOfType<KeyObject>();
        timeSinceLastPhoto = minTimeBetweenPhotos;
        isPaused = false;
    }

    // Update is called once per frame
    void Update() 
    {
        timeSinceLastPhoto += Time.deltaTime;
        if (!isPaused)
        {
            if (Input.GetMouseButtonDown(0) && timeSinceLastPhoto >= minTimeBetweenPhotos)
            {
                cameraButton.TakePicture();
                GetCamBounds();
                timeSinceLastPhoto = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                isPaused = false;
                UIManager.instance.ResetMenu(4);
            } else
            {
                isPaused = true;
                UIManager.instance.ResetMenu(12);
            }
        }
    }

    public void GetCamBounds()
    {
        Vector3[] _frustumCorners = new Vector3[4];
        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, _frustumCorners);
        for (int i = 0; i < 4; i++)
        {
            worldSpaceCorners[i].position = cam.transform.TransformVector(_frustumCorners[i]);
            // Debug.DrawRay(cam.transform.position, frustumCorners[i].position, Color.blue);
        }
        KeyObject _keyObject = null;
        foreach (KeyObject _object in keyObjects)
        {
            if (_object.CheckIfFound(worldSpaceCorners))
            {
                _keyObject = _object;
            }
        }
        UIManager.instance.TakePicture(_keyObject);
    }
}
