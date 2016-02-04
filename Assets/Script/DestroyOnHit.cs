using UnityEngine;
using System.Collections;

public class DestroyOnHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("environment"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
			if (!collision.gameObject.GetComponent<SymbiotState>().ShieldUp) {
            	collision.gameObject.GetComponent<PlayerEnergy>().spellEnergy = 0;
				Destroy(gameObject);
                Debug.Log("player hit");
                collision.gameObject.GetComponent<SymbiotController>().Stun();
			} else  {
				Vector3 v = GetComponent<Rigidbody> ().velocity;
				GetComponent<Rigidbody> ().velocity = -v; 
			}
        }
        else
        {
            Debug.Log(collision.gameObject.tag + "name" + collision.gameObject.name);
        }
    }
}
