using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideDoor : Interaction
{
    public bool isUnlocked = false;

    public override void Interact()
    {
        if (isUnlocked)
        {
            Debug.Log("Door is unlocked! Scene change triggered");
            SceneManager.LoadScene("Inside");
        }
        else
        {
            Debug.Log("The door is locked. You need a key.");
        }
    }
}
