using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtticKey : Interaction
{
    public AtticDoor atticDoor;
    public SceneManager sceneManager;
    public override void Interact()
    {
        if (atticDoor != null)
        {
            atticDoor.isUnlocked = true;
            Debug.Log("Attic key is picked up! Attic door is now unlocked.");
            Destroy(gameObject);
        }
    }
}
