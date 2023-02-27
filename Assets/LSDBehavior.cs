using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSDBehavior : MonoBehaviour
{
    public Transform cam;

    public float relativeMovementX = 0.3f;
    public float relativeMovementY = 0.3f;


    public float xOffset;
    public float yOffset;
    

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(cam.position.x * relativeMovementX + xOffset, cam.position.y * relativeMovementY + yOffset);

        transform.rotation = cam.rotation;
    }
}
