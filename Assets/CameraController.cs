using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objectToFocus;
    

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(objectToFocus.transform.position.x, objectToFocus.transform.position.y, -10);    

        transform.rotation = objectToFocus.transform.rotation;
    }
}
