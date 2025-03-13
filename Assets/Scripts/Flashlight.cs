using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlightLight; //Referencing light component added onto flashlight
    private bool isHeld = false;
    private bool isOn = false;


    private void PickupFlashlight()
    {
        isHeld = true;
        flashlightLight.gameObject.SetActive(true);
        Debug.Log("Flashlight picked up and ready to use");
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn;
        Debug.Log(isOn ? "Flashlight turned on" : "Flashlight turned off");
    }
}
