using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public GameObject poofPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySelf() {

        Instantiate(poofPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
