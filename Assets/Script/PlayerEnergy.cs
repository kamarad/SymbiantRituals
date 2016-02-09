using UnityEngine;
using System.Collections;

public class PlayerEnergy : MonoBehaviour {

	[SerializeField]
    private float _spellEnergy = 2f;
	public float spellEnergy    // Tussen 0 en 2
    {
        get { return _spellEnergy; }
        set
        {
            _spellEnergy = value;
            if (onEnergyChange != null) onEnergyChange(spellEnergy);

        }
    }


    public delegate void BurnOutEvent(bool bo);
    public delegate void ChargeChangeEvent(float newCharge);
    public event ChargeChangeEvent onEnergyChange;  // When the value of the energy changes
    public event BurnOutEvent onBurnout;    // When the user completely depletes the energy bar

    private int powers;          // tussen 0 en 3?
    public bool hasPowers
    {
        get
        {
            return (powers > 0);
        }
    }
    private bool drained = false;
    private float regenSpeed = 0.05f;   // 2 seconds to recharge fully
    private bool _burnout = false;
    private bool burnout
    {
        get
        {
            return _burnout;
        }
        set
        {
            _burnout = value;
            if (value)
            {
                burnoutTimer = 0f;
            }
            if (onBurnout != null) onBurnout(_burnout);

        }
    }
    private float burnoutTimer = 0f;
    public float burnoutDuration = 3f;
    
	public TEAMS team;

    public void Reset()
    {
        powers = 0;          // tussen 0 en 3?
        drained = false;
        regenSpeed = 0.05f;   // 2 seconds to recharge fully
        burnout = false;
    }

    // Update is called once per frame
	void LateUpdate()
    {
        // Do burnout timer
        if (burnout)
        {
            burnoutTimer += Time.deltaTime;
            if (burnoutTimer >= burnoutDuration)
            {
                burnout = false;    // timer automatically reset
            }
        }
        else
        {
            // No burnout, just normal state
            if (!drained)
                spellEnergy = Mathf.Clamp(spellEnergy + (Time.deltaTime * regenSpeed), 0f, 2f);   // Automatic recharge
            drained = false;
        }

        

    }

    public bool DrainCharge(float c)
    {
        drained = true;
        bool result = false;
        if (burnout) return false;
        if ((spellEnergy - c) < 0)
        {
            spellEnergy = 0;
            if (!burnout) burnout = true;
            result = false;
        } else
        {
            spellEnergy -= c;
            result = true;
        }

        // Fire event to update UI
        if (onEnergyChange != null) onEnergyChange(spellEnergy);

        return result;
    }

    public Color GetTeamColor()
    {
        if (team == TEAMS.ONE)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

    public void ReceiveSouls(int numOfSouls)
    {
        powers += numOfSouls;
        GetComponent<EnergyGlobes>().Activate(numOfSouls);
    }

    public int SacrificeSouls()
    {
        int souls = powers;
        powers = 0;
        GetComponent<EnergyGlobes>().Deactivate();
        return souls;
    }
}
