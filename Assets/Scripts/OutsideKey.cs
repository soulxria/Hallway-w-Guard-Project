using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideKey : Interaction
{
    public OutsideDoor outsideDoor; //Reference the door it unlocks

    public override void Interact()
    {
        if (outsideDoor != null)
        {
            outsideDoor.isUnlocked = true;
            Debug.Log("Key picked up! Door is now unlocked.");
            Destroy(gameObject);
        }
    }
}
