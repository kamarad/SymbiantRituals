using UnityEngine;
using System.Collections;

public class ChargeSpells : MonoBehaviour {

    public delegate void CoolDownEvent(bool onCoolDown);
    public event CoolDownEvent onChangeCoolDown;

    public delegate void SpellChargeEvent(CHARGESTATES cs);
    public event SpellChargeEvent onChargeStateChange;

    public GameObject spellProjectile;

    public PlayerEnergy playerEnergy;   // On same spells object

    public float charge
    {
        get
        {
            return Mathf.Clamp01(chargeTimer / timeToMaxCharge);
        }
    }

    public GameObject spellBorder_left;
    public GameObject spellBorder_right;

    public ParticleSystem spellSpray_particles;

    // Make spells on cooldown instead of charged
    private float cooldownTimer = 0f;


    public float spellborder_startAngle = 45f;
    public float spellborder_endAngle = 10f;

    public float startWidth = 0.2f;
    public float endWidth = 1f;

    public float timeToMaxCharge = 1.5f;
    private float chargeTimer = 0f;
    public enum CHARGESTATES
    {
        IDLE = 0,
        CHARGING,
        FULLCHARGE
    }
    private CHARGESTATES chargeState = CHARGESTATES.IDLE;


    void Start()
    {
        //playerEnergy = GetComponent<PlayerEnergy>();
        DeactivateCharge();
    }
	
	// Update is called once per frame
    public GameObject UseWand(string fireButton, bool fired)
    {
        GameObject bullet = HandleInput(fireButton, fired);

        // Update time
        if (chargeState == CHARGESTATES.CHARGING)
        {
            float drainAmount = Time.deltaTime / timeToMaxCharge;

                chargeTimer += drainAmount;
                if (chargeTimer >= 1f)
                {
                    chargeTimer = 1f;
                    chargeState = CHARGESTATES.FULLCHARGE;
                    if (onChargeStateChange != null) onChargeStateChange(CHARGESTATES.FULLCHARGE);
                }
        }

        // Calc angle between spellborders
        float newAngle = Mathf.Lerp(spellborder_startAngle, spellborder_endAngle, EaseOut(charge));
        // Now rotate the borders
        spellBorder_left.transform.localRotation = Quaternion.AngleAxis(-newAngle, Vector3.up);
        spellBorder_right.transform.localRotation = Quaternion.AngleAxis(newAngle, Vector3.up);
        // set width on borders
        spellBorder_left.transform.localScale = new Vector3(Mathf.Lerp(startWidth, endWidth, charge), 1f, 1f);
        spellBorder_right.transform.localScale = new Vector3(Mathf.Lerp(startWidth, endWidth, charge), 1f, 1f);
		return bullet;
    }

    GameObject HandleInput(string fireButton, bool fired)
    {
        // check controls, set state
        if (!fired && Input.GetButtonDown(fireButton))
        {
            if (chargeState == CHARGESTATES.IDLE)
            {
                ActivateCharge();
            }
        }
		if (!fired && Input.GetButtonUp(fireButton))
        {
            Debug.Log("Missile");
            return Fire();
        }
		return null;
    }

    GameObject Fire()
    {
        if (chargeState != CHARGESTATES.IDLE)
            {
                // Time to fire some shit!
                GameObject bullet = this.GetComponent<FireProjectile>().Fire(charge);

                // Then reset
                DeactivateCharge();
				return bullet;
            }
		return null;
    }

    void DeactivateCharge()
    {
        chargeState = CHARGESTATES.IDLE;
        spellBorder_left.SetActive(false);
        spellBorder_right.SetActive(false);
        if (onChargeStateChange != null) onChargeStateChange(CHARGESTATES.IDLE);
    }
    
    void ActivateCharge()
    {
        chargeState = CHARGESTATES.CHARGING;
        chargeTimer = 0f;
        spellBorder_left.SetActive(true);
        spellBorder_left.transform.localRotation = Quaternion.AngleAxis(-spellborder_startAngle, Vector3.up);
        spellBorder_left.transform.localScale = new Vector3(startWidth, 1f, 1f);
        spellBorder_right.SetActive(true);
        spellBorder_right.transform.localRotation = Quaternion.AngleAxis(spellborder_startAngle, Vector3.up);
        spellBorder_right.transform.localScale = new Vector3(startWidth, 1f, 1f);
        if (onChargeStateChange != null) onChargeStateChange(CHARGESTATES.CHARGING);
    }

    float EaseOut(float p)
    {
        //percent p
        return -1f * p * (p - 2f);
    }
}
