using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyObjectScript : FallingObjectScript
{
    public int energyValue = 1;

    protected override void OnCollected()
    {
        Debug.Log("Energy Collected!" + " Value: " + energyValue);
        base.OnCollected();
    }
}
