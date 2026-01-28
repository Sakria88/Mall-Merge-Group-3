using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonScript : MonoBehaviour
{
    //How many stars the energy cost
    public int starCost = 10;

    //How much energy the gplayer will get
    public int gainEnergy = 10;

    //References to item UI
    public StarUI starUI;
    public EnergyManager energyManager;
    //When the button is clicked this function will run
    public void Buy()
    {
        Debug.Log("GameManager Instance is: " + GameManagerScript.Instance);
        //Ask the game manager if the player can buy the energy
        bool bought = GameManagerScript.Instance.BuyEnergy(starCost, gainEnergy);

        //If the player does not have enough
        if (bought)
        {
            if (starUI != null)
                starUI.RefreshUI();

            if (energyManager != null)
                energyManager.RefreshUI();
            Debug.Log("Not enough stars");
        }
        else
        {
            Debug.Log("Not enough stars");
        }
    }
}
