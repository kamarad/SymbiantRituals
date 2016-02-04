using UnityEngine;
using System.Collections;

public class FireProjectile : MonoBehaviour {

    public float lowSpeed = 0.5f;
    public float highSpeed = 1f;

    public float lowWobble = 0f;
    public float highWobble = 0f;

    public float forwardOffset = 1f;

    public GameObject prefab_projectile;
    
    public GameObject Fire(float charge)
    {
        GameObject p = GameObject.Instantiate(prefab_projectile, transform.position, transform.rotation) as GameObject;
        p.transform.position += (transform.forward * forwardOffset);

        // Accelerate
        p.GetComponent<Rigidbody>().AddForce(transform.forward * Mathf.SmoothStep(lowSpeed, highSpeed, charge), ForceMode.Impulse);

        return p;
    }
}
