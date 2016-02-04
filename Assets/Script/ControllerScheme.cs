using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SchemeType
{
    NOT_SET,
    TWO_CONTROLLERS,
    FOUR_CONTROLLERS
}

public class ControllerUtil
{
    public enum Axis
    {
        FORWARD,
        STRAFE,
        TURN,
        BRAKE,
        ACCELERATE
    }

    public enum Button
    {
        A,
        B,
        X,
        Y,
        LTRIG,
        RTRIG
    }

    private static Dictionary<Axis, string> axisNames = new Dictionary<Axis, string>();
    private static Dictionary<Button, string> buttonNames = new Dictionary<Button, string>();

    static ControllerUtil()
    {
        axisNames.Add(Axis.FORWARD, "Forward");
        axisNames.Add(Axis.STRAFE, "Strafe");
        axisNames.Add(Axis.TURN, "Turn");
        axisNames.Add(Axis.BRAKE, "Brake");
        axisNames.Add(Axis.ACCELERATE, "Acc");

        buttonNames.Add(Button.A, "A");
        buttonNames.Add(Button.B, "B");
        buttonNames.Add(Button.X, "X");
        buttonNames.Add(Button.Y, "Y");
        buttonNames.Add(Button.LTRIG, "LTRIG");
        buttonNames.Add(Button.RTRIG, "RTRIG");
    }

    public static string GetName(Axis a)
    {
        return axisNames[a];
    }

    public static string GetName(Button b)
    {
        return buttonNames[b];
    }
}

public class SymbiotInputs
{
    public string golemStrafeAxis;
    public string golemForwardAxis;
    public string golemTurnAxis;
    public string golemDashButton;
    public string golemWallButton;

    public string wizardTurnAxis;
    public string wizardFireButton;
    public string wizardShieldButton;
}

public class ControllerScheme : MonoBehaviour
{

    public SchemeType controllerScheme = SchemeType.TWO_CONTROLLERS;
    public int[] controllerIds = new int[] {0, 1, 2, 3};

    [SerializeField]
    private ControllerUtil.Axis golemStrafeAxis = ControllerUtil.Axis.STRAFE;
    [SerializeField]
    private ControllerUtil.Axis golemForwardAxis = ControllerUtil.Axis.FORWARD;
    [SerializeField]
    private ControllerUtil.Axis golemTurnAxis = ControllerUtil.Axis.TURN;
    [SerializeField]
    private ControllerUtil.Button golemDashButton = ControllerUtil.Button.A;
    [SerializeField]
    private ControllerUtil.Button golemAltDashButton = ControllerUtil.Button.X;
    [SerializeField]
    private ControllerUtil.Button golemWallButton = ControllerUtil.Button.B;
    [SerializeField]
    private ControllerUtil.Button golemAltWallButton = ControllerUtil.Button.Y;

    [SerializeField]
    private ControllerUtil.Axis wizardTurnAxis = ControllerUtil.Axis.STRAFE;
    [SerializeField]
    private ControllerUtil.Button wizardFireButton = ControllerUtil.Button.A;
    [SerializeField]
    private ControllerUtil.Button wizardShieldButton = ControllerUtil.Button.B;

    [SerializeField]
    private string playerPrefix = "Player";

    private SymbiotInputs symbiantOne;
    private SymbiotInputs symbiantTwo;

    // Use this for initialization
	void Awake () {
        if (MenuController.schemeType != SchemeType.NOT_SET)
        {
            controllerScheme = MenuController.schemeType;
            controllerIds = MenuController.controllerIds;
        }
        SetControls();
	}

    private void SetControls()
    {
        if (controllerScheme == SchemeType.TWO_CONTROLLERS)
        {
            symbiantOne = createSingle(controllerIds[0], controllerIds[0]);
            symbiantTwo = createSingle(controllerIds[1], controllerIds[1]);
        } else if (controllerScheme == SchemeType.FOUR_CONTROLLERS)
        {
            symbiantOne = createDouble(controllerIds[0], controllerIds[1]);
            symbiantTwo = createDouble(controllerIds[2], controllerIds[3]);
        }
    }

    public SymbiotInputs GetInputs(TEAMS team)
    {
        if (team == TEAMS.ONE)
        {
            return symbiantOne;
        }
        else
        {
            return symbiantTwo;
        }
    }

    private SymbiotInputs createSingle(int golem, int wizard)
    {
        SymbiotInputs s = new SymbiotInputs();
        s.golemForwardAxis = ControllerUtil.GetName(golemForwardAxis) + " " + golem;
        //not used
        s.golemStrafeAxis = null;
        s.golemTurnAxis = ControllerUtil.GetName(golemStrafeAxis) + " " + golem;

        //Use alternative button
        s.golemDashButton = playerPrefix + " " + golem + " " + ControllerUtil.GetName(golemAltDashButton);
        s.golemWallButton = playerPrefix + " " + golem + " " + ControllerUtil.GetName(golemAltWallButton);

        // Use golem turn axis
        s.wizardTurnAxis = ControllerUtil.GetName(golemTurnAxis) + " " + wizard;
        s.wizardFireButton = playerPrefix + " " + wizard + " " + ControllerUtil.GetName(wizardFireButton);
        s.wizardShieldButton = playerPrefix + " " + wizard + " " + ControllerUtil.GetName(wizardShieldButton);

        return s;
    }

    private SymbiotInputs createDouble(int golem, int wizard)
    {
        SymbiotInputs s = new SymbiotInputs();
        s.golemForwardAxis = ControllerUtil.GetName(golemForwardAxis) + " " + golem;
        s.golemStrafeAxis = ControllerUtil.GetName(golemStrafeAxis) + " " + golem;
        s.golemTurnAxis = ControllerUtil.GetName(golemTurnAxis) + " " + golem;

        s.golemDashButton = playerPrefix + " " + golem + " " + ControllerUtil.GetName(golemDashButton);
        s.golemWallButton = playerPrefix + " " + golem + " " + ControllerUtil.GetName(golemWallButton);

        s.wizardTurnAxis = ControllerUtil.GetName(wizardTurnAxis)+ " " + wizard;
        s.wizardFireButton = playerPrefix + " " + wizard + " " + ControllerUtil.GetName(wizardFireButton);
        s.wizardShieldButton = playerPrefix + " " + wizard + " " + ControllerUtil.GetName(wizardShieldButton);

        return s;
    }

    // Update is called once per frame
	void Update () {
	}
}
