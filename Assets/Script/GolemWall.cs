using UnityEngine;
using System.Collections;

public class GolemWall : MonoBehaviour {

    public float damageFromProjectiles = 2f;

    public float wallLifetime = 8f;
    private float lifeTimer = 0f;

    public delegate void DestroyEvent();
    public event DestroyEvent onDestroy;
    

	// Update is called once per frame
	void Update () {
        lifeTimer += Time.deltaTime;
        CheckLife();
	}

    void OnCollisionEnter(Collision collision)
    {
        Projectile p = collision.collider.GetComponent<Projectile>();
        if (p != null)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        // add damage to life timer
        lifeTimer += damageFromProjectiles;
        CheckLife();
    }

    void CheckLife()
    {
        if (lifeTimer >= wallLifetime)
        {
            if (onDestroy != null) onDestroy();
            Destroy(gameObject);
        }
    }
}
