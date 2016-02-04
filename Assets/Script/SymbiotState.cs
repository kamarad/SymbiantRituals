using UnityEngine;
using System.Collections;

enum Abilities
{
    SHIELD,
    WALL,
    DASH,
    FIRE
} 

public class SymbiotState : MonoBehaviour {
    
    public delegate void CoolDownEvent(bool onCoolDown);
    public event CoolDownEvent onCDFire;
    public event CoolDownEvent onCDReflect;
    public event CoolDownEvent onCDWall;
    public event CoolDownEvent onCDDash;
    
    private static int ABILITY_COUNT = 4;

    [SerializeField]
    private GunController gun;

    [SerializeField]
    private float fireDuration = 2f;
    [SerializeField]
    private float fireCoolOffTime = 1.5f;
    [SerializeField]
    private float shieldDuration = 2f;
    [SerializeField]
    private float shieldCoolOffTime = 2f;
    [SerializeField]
    private float wallDuration = 10f;
    [SerializeField]
    private float wallCoolOffTime = 2f;
    [SerializeField]
    private float dashDuration = 2f;
    [SerializeField]
    private float dashCoolOffTime = 2f;
    [SerializeField]
    private GameObject shield;

    private float[] abilityTime = new float[4];
    private float[] coolTime = new float[4];

    private bool shieldUp;
    public bool ShieldUp
    {
        get
        {
            return shieldUp;
        }

        set
        {
            if (!shieldUp && coolTime[(int) Abilities.SHIELD] <= 0)
            {
                shieldUp = true;
                StartTimer(Abilities.SHIELD);
                shield.SetActive(true);
            }

        }
    }

    private bool wallUp;
    public bool WallUp
    {
        get
        {
            return wallUp;
        }

        set
        {
            if (!wallUp && coolTime[(int)Abilities.WALL] <= 0)
            {
                wallUp = true;
                StartTimer(Abilities.WALL);
            }
        }
    }

    private bool dashOn;
    public bool DashOn
    {
        get
        {
            return dashOn;
        }

        set
        {
            if (!dashOn && coolTime[(int)Abilities.DASH] <= 0)
            {
                dashOn = true;
                StartTimer(Abilities.DASH);
            }

        }
    }

    private bool fired = false;
    public bool Fired
    {
        get { return fired; }
        set
        {
            if (!fired && coolTime[(int)Abilities.FIRE] <= 0)
            {
                fired = true;
                StartTimer(Abilities.FIRE);
                GetComponent<SymbiotController>().wizard.GetComponent<AudioSource>().Play();
            }

        }
    }

    private void StartTimer(Abilities a)
    {
        float val = 0f;
        switch (a)
        {
            case Abilities.SHIELD:
                {
                    val = shieldDuration;
                    break;
                }
            case Abilities.WALL:
                {
                    val = wallDuration;
                    break;
                }
            case Abilities.DASH:
                {
                    val = dashDuration;
                    break;
                }
            case Abilities.FIRE:
                {
                    val = fireDuration;
                    break;
                }
        }
        abilityTime[(int)a] = val;
    }

	// Use this for initialization
	void Start () {
        gun.symState = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (shieldUp)
        {
            abilityTime[0] -= Time.deltaTime;
            shieldUp = abilityTime[0] > 0;
            if (!shieldUp)
            {
                shield.SetActive(false);
                if (onCDReflect != null) onCDReflect(true);
                coolTime[0] = shieldCoolOffTime;
            }
        }

        if (wallUp)
        {
            abilityTime[1] -= Time.deltaTime;
            wallUp = abilityTime[1] > 0;
            if (!wallUp)
            {
                if (onCDWall != null) onCDWall(true);
                coolTime[1] = wallCoolOffTime;
            }
        }

        if (dashOn)
        {
            abilityTime[2] -= Time.deltaTime;
            dashOn = abilityTime[2] > 0;
            if (!dashOn)
            {
                if (onCDDash != null) onCDDash(true);
                coolTime[2] = dashCoolOffTime;
            }
        }

        if (fired)
        {
            abilityTime[3] -= Time.deltaTime;
            fired = abilityTime[3] > 0;
            if (!fired)
            {
                if (onCDFire != null) onCDFire(false);
                coolTime[3] = fireCoolOffTime;
            }
        }

        for (int i = 0; i < ABILITY_COUNT; i++)
        {
            if (coolTime[i] > 0)
            {
                coolTime[i] -= Time.deltaTime;
                if (coolTime[i] <= 0)
                {
                    switch (i)
                    {
                        case 0:
                            {
                                if (onCDReflect != null) onCDReflect(false);
                                break;
                            }
                        case 1:
                            {
                                if (onCDWall != null) onCDWall(false);
                                break;
                            }
                        case 2:
                            {
                                if (onCDDash != null) onCDDash(false);
                                break;
                            }
                        case 3:
                            {
                                if (onCDFire != null) onCDFire(true);
                                break;
                            }
                    }
                }
            }
        }
	}
}
