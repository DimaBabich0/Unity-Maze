using System.Collections.Generic;
using UnityEngine;

public class CameraFixedScript : MonoBehaviour
{
    private List<Transform> cameraPositions = new List<Transform>();

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            cameraPositions.Add(obj);
        }
        CameraScript.fixedCameraPosition = cameraPositions[0].transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CameraScript.isFixed = !CameraScript.isFixed;
        }

        if (CameraScript.isFixed)
        {
            for (int i = 0; i < cameraPositions.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    //Debug.Log($"Pressed button: {i + 1}; Name: {obj.name} {obj.position}");
                    Transform obj = cameraPositions[i].transform;
                    CameraScript.fixedCameraPosition = obj;
                }
            }
        }
    }
}
