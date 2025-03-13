using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AtticDoor : Interaction
{
    public bool isUnlocked = false;
    public SceneManager sceneManager;

    public override void Interact()
    {
        if (isUnlocked)
        {
            Debug.Log("Inside door unlocked! Scene change triggered");
            sceneManager.LoadNextScene("Attic");
        }

        else
        {
            Debug.Log("This attic door is locked. You need a key");
        }
    }
}
