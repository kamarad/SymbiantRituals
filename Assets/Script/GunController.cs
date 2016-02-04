using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

    [SerializeField]
    private float speed;
    private string wizardFireButton;
    private ChargeSpells wand;

    public SymbiotState symState;
    public GunInfo gunProperties;


    void Start () {
        wand = GetComponent<ChargeSpells>();
	}
	
	// Update is called once per frame
	void Update () {
        GameObject bullet = wand.UseWand(wizardFireButton, symState.Fired);
        if (bullet != null)
        {
            symState.Fired = true;
            Debug.Log(gunProperties.wobbleSpread);
//            bullet.GetComponent<BulletWobble>().BalisticProps = gunProperties;
  //          Destroy(bullet, 5);
        }
    }

    public void SetInputs(SymbiotInputs inputs)
    {
        wizardFireButton = inputs.wizardFireButton;
    }
}

[System.Serializable]
public class GunInfo
{
    public float wobbleSpread = .1f;
    public float wobbleSpeed = .15f;
    public float minImpulse = -1f;
    public float maxImpulse = 1f;
}
