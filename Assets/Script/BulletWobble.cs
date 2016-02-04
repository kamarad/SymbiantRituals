using UnityEngine;
using System.Collections;

public class BulletWobble : MonoBehaviour {

    [SerializeField]
    private float currentImpulse = 0;
    private Rigidbody body;
    private float step;

    [SerializeField]
    private GunInfo balisticProps;

    public GunInfo BalisticProps
    {
        get
        {
            return balisticProps;
        }
        set
        {
            balisticProps = value;
            Debug.Log(balisticProps.wobbleSpread + "," + value.wobbleSpread);
            step = value.wobbleSpeed;
        }
    }

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (body != null) {
            body.AddRelativeForce(Vector3.left * currentImpulse * balisticProps.wobbleSpread, ForceMode.Impulse);

            if (currentImpulse <= balisticProps.minImpulse || currentImpulse >= balisticProps.maxImpulse)
            {
                step *= -1;
            }
            currentImpulse += step;
        }
	}
}
