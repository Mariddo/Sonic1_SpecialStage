using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialStageUIBehavior : MonoBehaviour
{
    
    public static SpecialStageUIBehavior instance;

    private void Awake() {

        if(instance == null) {

            instance = this;
        } else {
            Destroy(gameObject);
        }
        
    }
    
    
    public TMP_Text pickupText;

    public int stringLength = 3;

    public void UpdatePickupText(int value) {

        pickupText.text = PrependStringWithZeros(value);
    }

    string PrependStringWithZeros(int passedValue) {

        string newString = passedValue + "";

        int passedStringLength = (newString).Length;

        for (int x = 0 ; x < stringLength - passedStringLength ; x++) {
            newString = "0" + newString;
        }

        return newString;
    }
}
