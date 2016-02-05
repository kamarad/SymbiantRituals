using UnityEngine;
using System.Collections;

public class SymbiotController : MonoBehaviour {

	public float force;

    [SerializeField]
    private AudioClip stunAudio;
    [SerializeField]
    private AudioClip shieldAudio;

    [SerializeField]
    private float dashForce = 3f;
    [SerializeField]
    private float golemTurnRate = 1f;
    [SerializeField]
    private float moveCycleTime = 2f;
    [SerializeField]
	private float forwardStrafeRatio = 0.7f;
	[SerializeField]
	private float leftWizardAngle = 75f;
	[SerializeField]
	private float rightWizardAngle = 75f;

	private string golemForwardAxis;
    private string golemStrafeAxis;
    private string golemTurnAxis;
    private string golemDashButton;

    private string wizardTurnAxis;
    private string wizardShieldButton;

    [SerializeField]
    private float wizardTurnRate = 4f;

    private Transform golem;
    private GolemCreateWall wallCreator;
    //HACK use public to give sym state access to audio source
    public Transform wizard;
    private GunController gun;
    private SymbiotState symState;

    private bool stun;
    private float walkAngle;

    [SerializeField]
    private float stdStunTime = 3f;
    private float stunTime;

    void Start()
	{
		golem = transform.Find("Golem");
        wallCreator = golem.Find("wallcreator").GetComponent<GolemCreateWall>();

		wizard = transform.Find("Wizard");
        gun = wizard.Find("Gun").Find("Spells").GetComponent<GunController>();

        symState = GetComponent<SymbiotState>();

        SetInputs(GameObject.Find("GameManager").GetComponent<ControllerScheme>().GetInputs(GetComponent<PlayerEnergy>().team));
	}

	// Update is called once per frame
	void Update () {
        if (stun)
        {
            stunTime -= Time.deltaTime;
            stun = stunTime > 0;
        }
        else
        {
            DoGolemAbilities();
            DoGolemMovement();
            DoWizardMovement();
            DoWizardAbilities();
        }
	}

    private void DoGolemAbilities()
    {
        if (Input.GetButtonDown(golemDashButton))
        {
            symState.DashOn = true;
            if (symState.DashOn)
            {
                golem.GetComponent<GolemAudio>().Dash();
            }
        }
    }

	private void DoWizardAbilities()
	{
		if (Input.GetButtonDown(wizardShieldButton))
		{
			symState.ShieldUp = true;
		}
	}

	void DoGolemMovement()
	{
        if (symState.DashOn)
        {
            DoDash();
            golem.gameObject.GetComponent<Animator>().SetBool("dash", true);
        }
        else
        {
            golem.gameObject.GetComponent<Animator>().SetBool("dash", false);
            float move = Input.GetAxisRaw(golemForwardAxis);
            float strafe = 0;
            if (golemStrafeAxis != null)
            {
                strafe = Input.GetAxisRaw(golemStrafeAxis) * forwardStrafeRatio;
            }
            float turn = Input.GetAxisRaw(golemTurnAxis);

            Vector3 forwardImpluse = move * golem.transform.forward.normalized;
            forwardImpluse *= force * Mathf.Abs(Mathf.Cos(walkAngle));
            Vector3 sideImpluse = strafe * golem.transform.right.normalized;
            sideImpluse *= force;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(forwardImpluse + sideImpluse, ForceMode.Impulse);

            if (move != 0f) {
                if (!golem.gameObject.GetComponent<Animator>().GetBool("walk")) {
                    golem.gameObject.GetComponent<Animator>().SetBool("halt", false);
                    golem.gameObject.GetComponent<Animator>().SetBool("walk", true);
                    walkAngle = 0f;
                }
                else
                {
                    walkAngle += Mathf.PI * (2 / moveCycleTime) *move * Time.deltaTime;
                }
            }
            else
            {
                golem.gameObject.GetComponent<Animator>().SetBool("halt", true);
                golem.gameObject.GetComponent<Animator>().SetBool("walk", false);
            }
            // now rotation
            if (turn != 0f)
            {
                float angle = golemTurnRate * turn;
                golem.Rotate(Vector3.up, angle, Space.Self);
                CheckWizard();
            }
        }

	}

    private void DoDash()
    {
        float move = dashForce;

        Vector3 forwardImpluse = move * golem.transform.forward.normalized;
        forwardImpluse *= force;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(forwardImpluse, ForceMode.Impulse);

        CheckWizard();
    }

	private void CheckWizard()
	{
		float currAngle = Vector3.Angle(golem.forward, wizard.forward) * Direction();
		float nextAngle = Mathf.Clamp(currAngle, -leftWizardAngle, rightWizardAngle);
		if (nextAngle != currAngle)
		{
			if (nextAngle < currAngle)
			{
				RotateGun(-1);
			}
			else
			{
				RotateGun(1);
			}
		}
	}

	void DoWizardMovement()
	{

		float turn = Input.GetAxisRaw(wizardTurnAxis);

		// Rotate the rig (the root object) around Y axis only:
		// now rotation
		if (turn != 0f)
		{
			RotateGun(turn);
		}
	}

	private void RotateGun(float turn)
	{
		float dir = Direction();
		float currAngle = Vector3.Angle(golem.forward, wizard.forward) * dir;
		float angle = turn * wizardTurnRate;
		float nextAngle = Mathf.Clamp(currAngle + angle, -leftWizardAngle, rightWizardAngle);

		//            angle = Mathf.Min(angle, nextAngle - currAngle);
		//          Debug.Log("correction" + angle);
		if ((dir < 0 && (nextAngle < currAngle || angle > 0)) || (dir > 0 && (nextAngle > currAngle || angle < 0)))
		{
			wizard.Rotate(Vector3.up, angle, Space.Self);
		}
	}

	private float Direction()
	{
		float angle = Vector3.Angle(golem.right, wizard.forward);
		return (angle > 90 ? -1 : 1);
	}

    public void Stun()
    {
        stun = true;
        stunTime = stdStunTime;
        golem.gameObject.GetComponent<Animator>().SetBool("halt", true);
        golem.gameObject.GetComponent<Animator>().SetBool("walk", false);

        GetComponent<AudioSource>().Play();
    }

    private void SetInputs(SymbiotInputs inputs)
    {
        golemForwardAxis = inputs.golemForwardAxis;
        golemStrafeAxis = inputs.golemStrafeAxis;
        golemTurnAxis = inputs.golemTurnAxis;

        golemDashButton = inputs.golemDashButton;
        wallCreator.SetInputs(inputs);

        wizardTurnAxis = inputs.wizardTurnAxis;
        wizardShieldButton = inputs.wizardShieldButton;
        gun.SetInputs(inputs);
    }
}
