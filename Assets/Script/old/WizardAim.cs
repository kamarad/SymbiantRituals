using UnityEngine;
using System.Collections;

public class WizardAim : MonoBehaviour {

    public Vector3 aimDirection;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetAimDirection();
	}

    void GetAimDirection()
    {
        aimDirection = new Vector3(
            Input.GetAxis("AimHorizontal"),
            0f,
            Input.GetAxis("AimVertical")
            ).normalized;

        transform.rotation = Quaternion.LookRotation(aimDirection);
    }
}
