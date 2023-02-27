using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldController : MonoBehaviour
{
    public Color flashColor1;
    public Color flashColor2;

    public float flashRate = 0.25f;
    
    float flashTimer;
    int color;

    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        flashTimer = 0;
        sr = GetComponent<SpriteRenderer>();
        color = 0;
    }

    // Update is called once per frame
    void Update()
    {
        flashTimer += Time.deltaTime;
        if(flashTimer >= flashRate) {
            ChangeColor();
            flashTimer = 0;
        }
    }

    void ChangeColor() {
        if(color == 0) {
            sr.color = flashColor2;
            color = 1;
        } else {
            sr.color = flashColor1;
            color = 0;
        }
    }
}
