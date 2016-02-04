using UnityEngine;
using System.Collections;

public class SpellParticles : MonoBehaviour {

    public ChargeSpells chargeSpells;
    ParticleSystem ps;

    public float startSpeed = 1f;
    public float endSpeed = 5f;
    public Color chargedColor;
    public Color chargingColor;

    // Use this for initialization
	void Start () {
        ps = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnChargeStateChangeHandler(ChargeSpells.CHARGESTATES chargeState)
    {
        switch (chargeState)
        {
            case ChargeSpells.CHARGESTATES.IDLE:
                ParticleSystem.EmissionModule em = ps.emission;
                em.enabled = false;
                break;
            case ChargeSpells.CHARGESTATES.CHARGING:
                ps.startSpeed = startSpeed;
                ps.startColor = chargingColor;
                ParticleSystem.EmissionModule em1 = ps.emission;
                em1.enabled = true;
                break;

            case ChargeSpells.CHARGESTATES.FULLCHARGE:
                ps.startSpeed = endSpeed;
                ps.startColor = chargedColor;
                break;
        }
    }
}
