using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AtticDoor : Interaction
{
    public bool isUnlocked = false;

    public override void Interact()
    {
        if (isUnlocked)
        {
            Debug.Log("Inside door unlocked! Scene change triggered");
            SceneManager.LoadScene("Attic");
        }

        else
        {
            Debug.Log("This attic door is locked. You need a key");
        }
    }
}
