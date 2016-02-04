using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerEnergy))]
public class UseShrines : MonoBehaviour {

    private Shrine currentShrine;

    void OnTriggerEnter(Collider collider)
    {
        Shrine s = collider.GetComponent<Shrine>();
        if (s != null && s.Occupy(gameObject.GetComponent<PlayerEnergy>()))
        {
            currentShrine = s;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Clear current altar property
        Shrine s = collider.GetComponent<Shrine>();
        if (s != null)
        {
            // make the capture cancel if you leave the trigger
            if (currentShrine != null)
            {
                currentShrine.Leave(gameObject.GetComponent<PlayerEnergy>());
                currentShrine = null;
            }
        }
    }
}
