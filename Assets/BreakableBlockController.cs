using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct BreakableBlockState {
    public Color color;
    public float value;
}

public class BreakableBlockController : MonoBehaviour
{
    
    public float health = 4f;

    public BreakableBlockState [] stateList;

    public bool beingBroken;

    public int currentState;
    
    SpriteRenderer sr;

    public float touchDamageBonus = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        beingBroken = false;

        currentState = stateList.Length - 1;

        sr = GetComponent<SpriteRenderer>();

        sr.color = stateList[currentState].color;
    }

    // Update is called once per frame
    void Update()
    {

        if(beingBroken) {
            health -= Time.deltaTime;

            if(health <= 0f) {
                Destroy(gameObject);
            }
            else if(health < stateList[currentState].value) {
                currentState--;

                sr.color = stateList[currentState].color;
            }
        }

        
    }

    void SelectCurrentState() {


    }

    private void OnCollisionEnter2D(Collision2D other)  {

        if(other.gameObject.tag == "Player") {
            Debug.Log("Player is breaking block");
            beingBroken = true;
            health -= touchDamageBonus;
        }
    }

    private void OnCollisionExit2D(Collision2D other)  {

        if(other.gameObject.tag == "Player") {
            Debug.Log("Player is not breaking block");
            beingBroken = false;
        }
    }
        

}
