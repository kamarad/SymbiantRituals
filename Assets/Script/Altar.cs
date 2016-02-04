using UnityEngine;
using System.Collections;

public class Altar : MonoBehaviour {

    public GameManager gameManager;
    
    public void TurnIn(PlayerEnergy playerEnergy)
    {
        // Get soals from pEnergy, reset to 0 instantly
        int amountAdded = playerEnergy.SacrificeSouls();
        gameManager.AddScore(playerEnergy.team, amountAdded);
        Debug.Log("Turn in ok");

        // TODO: Trigger animations
    }
}
