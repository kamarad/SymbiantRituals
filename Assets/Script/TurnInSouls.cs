using UnityEngine;
using System.Collections;

public class TurnInSouls : MonoBehaviour {


    private PlayerEnergy playerEnergy;
    private Altar altar;

	void Start() {
		playerEnergy = gameObject.GetComponent<PlayerEnergy> ();
    }

    // Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger" + playerEnergy);
        Altar a = collider.gameObject.GetComponent<Altar>();
        if (a != null)
        {
            altar = a;
            altar.TurnIn(playerEnergy);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        Altar a = collider.gameObject.GetComponent<Altar>();
        if (a != null)
        {
            altar = null;
        }
    }
}
