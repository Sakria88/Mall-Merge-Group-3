using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObjectScript : FallingObjectScript
{
    public int energyValue = -5;

    protected override void OnCollected()
    {
        Debug.Log("Boooooom!" + " Value: " + energyValue);
        base.OnCollected();
    }
}