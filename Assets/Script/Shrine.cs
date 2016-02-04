using UnityEngine;
using System.Collections;

public class Shrine : MonoBehaviour {

    [SerializeField]
    private int numSouls = 3;

    [SerializeField]
    private float captureDuration = 3f;
    public float CaptureDuration
    {
        get
        {
            return captureDuration;
        }
    }

    private float captureTimer = 0f; // 0-1 percent
	private float captureProgress
	{
		get { return Mathf.Clamp01(captureTimer / captureDuration); }
	}

    [SerializeField]
    internal float timeoutDuration = 10f;
    private float timeoutTimer = 0f;

    [SerializeField]
    private Light captureLight;
    [SerializeField]
    private Color hasPowerColor;

    private bool resetTimer;
    private ShrineFSM fsm;
    private PlayerEnergy player;

    internal ShrineLight[] lights;
	internal bool hasPower = false;
    internal bool occupied;

	// Use this for initialization
	void Start () {
        lights = GetComponentsInChildren<ShrineLight>();
        fsm = new ShrineFSM(this);
        fsm.reset();
	}

	// Update is called once per frame
	void Update () {
        fsm.updateMachine(Time.deltaTime);

        if (resetTimer)
        {
            timeoutTimer += Time.deltaTime;
            if (timeoutTimer >= timeoutDuration)
            {
                //timeout, disable shrine
                Reset();
                resetTimer = false;
            }
        }
	}

    internal void StartResetTimer()
    {
        resetTimer = true;
        timeoutTimer = 0;
    }

    public bool IsReadyForPower()
    {
        int state = fsm.getCurrentState().getType();
        return state == ShrineState.EMPTY || state == ShrineState.OCCUPIED;
    }

    public void GivePower()
    {
        hasPower = true;
        captureLight.enabled = true;
        captureLight.color = hasPowerColor;
    }
    
    public bool Occupy(PlayerEnergy player)
	{
        if (!occupied)
        {
            this.player = player;
            occupied = true;
            return true;
        }
        return false;
    }

    public void Leave(PlayerEnergy player)
    {
        if (this.player != null && this.player.Equals(player))
        {
            this.player = null;
            occupied = false;
        }
    }

	internal void Reset()
	{
        foreach (ShrineLight l in lights)
        {
            l.flicker = true;
        }
        captureLight.enabled = false;
        captureLight.color = Color.black;
        hasPower = false;
        occupied = false;
        player = null;

	}

	internal void FinishCapture()
	{
        hasPower = false;
		captureLight.color = player.GetTeamColor();
        player.ReceiveSouls(numSouls);
        StartCoroutine(ShrineLight.FadeInLight(captureLight, captureLight.intensity));
    }

}

class ShrineFSM : FSMMachine<ShrineState>
{
    class Empty : ShrineState
    {
        public Empty(Shrine shrine)
            : base(shrine)
        {
            this.type = EMPTY;
        }

        public override int checkTransitions()
        {
            if (shrine.occupied) return OCCUPIED;
            if (shrine.hasPower) return POWERED;
            return type;
        }

        public override void enter()
        {
            base.enter();
            shrine.Reset();
        }
    }

    class Powered : ShrineState
    {
        public Powered(Shrine shrine)
            : base(shrine)
        {
            this.type = POWERED;
        }

        public override int checkTransitions()
        {
            if (shrine.occupied) return CAPTURING;
            return type;
        }
    }

    class Capturing : ShrineState
    {
        private int lightsOn;

        public Capturing(Shrine shrine)
            : base(shrine)
        {
            this.type = CAPTURING;
        }

        public override int checkTransitions()
        {
            if (nextTurn()) return CAPTURED;
            if (!shrine.occupied) return POWERED;
            return type;
        }

        public override void enter()
        {
            base.enter();
            setDelay(shrine.CaptureDuration);
            startTimer();
            lightsOn = 0;
        }

        public override void update(float delta)
        {
            LightsProgress(timeSinceStart());
        }

        private void LightsProgress(float p)
        {
            float interval = (shrine.lights.Length+1) / shrine.CaptureDuration;
            int numLightsToActivate = Mathf.FloorToInt(p * interval);
            // Activate the lights if not activated already
            if (numLightsToActivate > lightsOn)
            {
                for (int i = lightsOn; i < numLightsToActivate; i++)
                {
                    if (i < shrine.lights.Length)
                    {
                        shrine.lights[i].FadeIn();
                    }
                    lightsOn++;
                }
            }
        }

    }

    class Captured : ShrineState
    {
        public Captured(Shrine shrine)
            : base(shrine)
        {
            this.type = CAPTURED;
        }

        public override int checkTransitions()
        {
            if (!shrine.occupied) return WAIT;
            return type;
        }

        public override void enter()
        {
            base.enter();
            shrine.FinishCapture();
        }
    }

    class Occupied : ShrineState
    {
        public Occupied(Shrine shrine)
            : base(shrine)
        {
            this.type = OCCUPIED;
        }

        public override int checkTransitions()
        {
            if (!shrine.occupied) return EMPTY;
            if (shrine.hasPower) return POWERED;
            return type;
        }
    }

    class Wait : ShrineState
    {
        public Wait(Shrine shrine)
            : base(shrine)
        {
            this.type = WAIT;
        }

        public override int checkTransitions()
        {
            if (nextTurn()) return EMPTY;
            return type;
        }

        public override void enter()
        {
            base.enter();
            setDelay(shrine.timeoutDuration);
            startTimer();
        }
    }

    public ShrineFSM(Shrine shrine)
        : base("Shrine FSM")
    {
        ShrineState empty = new Empty(shrine);
        addState(empty);
        addState(new Powered(shrine));
        addState(new Capturing(shrine));
        addState(new Captured(shrine));
        addState(new Occupied(shrine));
        addState(new Wait(shrine));
        setDefaultState(empty);
    }

}


abstract class ShrineState : FSMState
{
    public static short EMPTY = 0;
    public static short POWERED = 1;
    public static short CAPTURING = 2;
    public static short CAPTURED = 3;
    public static short OCCUPIED = 4;
    public static short WAIT = 5;

    protected Shrine shrine;

    public ShrineState(Shrine shrine)
        : base()
    {
        this.shrine = shrine;
    }

    public override void enter()
    {
        Debug.Log("entering" + type + " for " + shrine.gameObject.name);
    }

    public override void exit()
    {

    }
    public override void update(float delta)
    {

    }
    public override void init()
    {

    }
}