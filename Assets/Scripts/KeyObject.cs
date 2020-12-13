using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    // Public variables
    public float requiredProximity = 10f;
    public string keyObjectName;

    // Private variables
    private GameObject[] frustumPoints;
    private GameObject refPoint;
    private bool isFound;

    // Start is called before the first frame update
    void Start() 
    {
        isFound = false;
        frustumPoints = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            frustumPoints[i] = new GameObject();
            frustumPoints[i].transform.SetParent(GameManager.instance.cam.transform);
            frustumPoints[i].name = gameObject.name + "frustumPoint" + i.ToString();
        }
        refPoint = new GameObject();
        refPoint.transform.SetParent(GameManager.instance.cam.transform);
        refPoint.name = gameObject.name + "refPoint";
    }

    // Update is called once per frame
    void Update() {}

    public bool CheckIfFound(Transform[] _frustumCorners)
    {
        if (!isFound)
        {
            // check if object is within frustum points
            int _trueCount = 0;
            refPoint.transform.position = transform.position;
            if (refPoint.transform.localPosition.z > 0)
            {
                Plane _plane = new Plane(GameManager.instance.cam.transform.forward, transform.position);
                for (int i = 0; i < 4; i++) if (CheckFrustumPoint(_plane, _frustumCorners, i)) _trueCount++;
            }
            // check if object is within range
            if ((transform.position - GameManager.instance.cam.transform.position).magnitude <= requiredProximity) _trueCount++;
            // check if object is blocked
            int _layerMask = 1 << 9;
            _layerMask = ~_layerMask;
            RaycastHit _hit;
            if (Physics.Raycast(GameManager.instance.cam.transform.position, (transform.position - GameManager.instance.cam.transform.position).normalized, out _hit, Mathf.Infinity, _layerMask))
            {
                if (_hit.collider.name == gameObject.name) _trueCount++;
            }
            // debug statements
            if (_trueCount == 6)
            {
                isFound = true;
                return true;
            }
        }
        return false;
    }

    public bool CheckFrustumPoint(Plane _plane, Transform[] _frustumCorners, int _index)
    {
        Ray _ray = new Ray(GameManager.instance.cam.transform.position, _frustumCorners[_index].transform.position);
        float _enter = 0.0f;
        if (_plane.Raycast(_ray, out _enter))
        {
            Vector3 hitPoint = _ray.GetPoint(_enter);
            frustumPoints[_index].transform.position = hitPoint;
            if (_index == 0 && refPoint.transform.localPosition.y > frustumPoints[_index].transform.localPosition.y && refPoint.transform.localPosition.x > frustumPoints[_index].transform.localPosition.x) return true;
            if (_index == 1 && refPoint.transform.localPosition.y < frustumPoints[_index].transform.localPosition.y && refPoint.transform.localPosition.x > frustumPoints[_index].transform.localPosition.x) return true;
            if (_index == 2 && refPoint.transform.localPosition.y < frustumPoints[_index].transform.localPosition.y && refPoint.transform.localPosition.x < frustumPoints[_index].transform.localPosition.x) return true;
            if (_index == 3 && refPoint.transform.localPosition.y > frustumPoints[_index].transform.localPosition.y && refPoint.transform.localPosition.x < frustumPoints[_index].transform.localPosition.x) return true;
        }
        return false;
    }
}

/*
USEFUL PROJECTION CODE

Vector3 _camToObject = transform.position - GameManager.instance.cam.transform.position;
Vector3 _camToObjectProjection = Vector3.Project(_camToObject, GameManager.instance.cam.transform.forward);
Vector3 _projectionPosition = GameManager.instance.cam.transform.position + _camToObjectProjection;
projection.transform.position = _projectionPosition;
*/