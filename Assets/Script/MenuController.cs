using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    public static SchemeType schemeType = SchemeType.NOT_SET;
    public static int[] controllerIds;

    private static MenuController singleton;

    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private GameObject controllerScheme;
    [SerializeField]
    private GameObject controllerSelection;
    [SerializeField]
    private GameObject controllerSetup;
    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private string gameScene;

    private GameObject[] menus = new GameObject[5];

    private MenuFSM fsm = new MenuFSM();

	// Use this for initialization
	void Start () {
        MenuController.singleton = this;
        menus[0] = startMenu;
        menus[1] = controllerScheme;
        menus[2] = controllerSelection;
        menus[3] = controllerSetup;
        menus[4] = LoadingScreen;
        fsm.reset();
    }

    void Update()
    {
        fsm.updateMachine(Time.deltaTime);
    }

    internal static void enableMenu(int id)
    {
        for (int i = 0; i < singleton.menus.Length; i++)
        {
            singleton.menus[i].SetActive(i == id);
        }
    }

    internal static void SwitchIcons(string side)
    {
        Debug.Log(singleton.controllerSetup.transform.Find("Controller menu R"));
        Image p1 = singleton.controllerSetup.transform.Find("Controller menu " + side + "/Character Select " + side + "/Golem").GetComponent<Image>();
        Image p2 = singleton.controllerSetup.transform.Find("Controller menu " + side + "/Character Select " + side + "/Wizard").GetComponent<Image>();

        Sprite tmp = p1.sprite;
        p1.sprite = p2.sprite;
        p2.sprite = tmp;
    }

    internal static void LockIcons(string side, bool lockChoice)
    {
        Image p1 = singleton.controllerSetup.transform.Find("Controller menu " + side + "/Character Select " + side + "/Golem").GetComponent<Image>();
        Image p2 = singleton.controllerSetup.transform.Find("Controller menu " + side + "/Character Select " + side + "/Wizard").GetComponent<Image>();

        Color c = (lockChoice ? Color.gray : Color.white);
        p1.color = c;
        p2.color = c;
    }

    internal static void ConfigureControllers()
    {
        if (schemeType == SchemeType.TWO_CONTROLLERS)
        {
            singleton.controllerSelection.transform.Find("Controller menu R/Player 2").gameObject.SetActive(false);
            singleton.controllerSelection.transform.Find("Controller menu L/Player 2").gameObject.SetActive(true);
            singleton.controllerSelection.transform.Find("Controller menu L/Player 3").gameObject.SetActive(false);
            singleton.controllerSelection.transform.Find("Controller menu L/Player 4").gameObject.SetActive(false);
        }
        else
        {
            singleton.controllerSelection.transform.Find("Controller menu R/Player 2").gameObject.SetActive(true);
            singleton.controllerSelection.transform.Find("Controller menu L/Player 2").gameObject.SetActive(false);
            singleton.controllerSelection.transform.Find("Controller menu L/Player 3").gameObject.SetActive(true);
            singleton.controllerSelection.transform.Find("Controller menu L/Player 4").gameObject.SetActive(true);
        }
    }

    internal static void LockController(int index) 
    {
        string side = (index / 2 == 1 ? "L" : "R");
        string buttonImageObj = "Controller menu " + side + "/Player " + (index+1);
        singleton.controllerSelection.transform.Find(buttonImageObj).GetChild(1).GetComponent<Image>().color = Color.gray;
    }

    internal static void StartGame()
    {
        SceneManager.LoadSceneAsync(singleton.gameScene);
    }
}

class MenuFSM : FSMMachine<MenuState>
{
    public MenuFSM() : base("Menu FSM")
    {
        StartScreen s = new StartScreen();
        addState(s);
        addState(new SchemeState());
        addState(new SelectionState());
        addState(new SetupState());
        addState(new LoadingState());
        setDefaultState(s);
    }
}

class StartScreen : MenuState
{
    internal StartScreen()
    {
        this.type = START;
    }

    public override int checkTransitions()
    {
        if (Input.GetButtonUp("Submit")) return SCHEME;
        return type;
    }

    public override void enter()
    {
        base.enter();
        MenuController.controllerIds = new int[] {-1, -1, -1, -1};
    }

    public override void update(float delta)
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }
}

class SchemeState : MenuState
{
    private bool chosen;

    internal SchemeState()
    {
        this.type = SCHEME;
    }

    public override int checkTransitions()
    {
        if (chosen) return SELECTION;
        return type;
    }

    public override void enter()
    {
        base.enter();
        chosen = false;
    }

    public override void update(float delta)
    {
        if (Input.GetButtonDown("Submit"))
        {
            MenuController.schemeType = SchemeType.TWO_CONTROLLERS;
            chosen = true;
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            MenuController.schemeType = SchemeType.FOUR_CONTROLLERS;
            chosen = true;
        }
    }
}

class SelectionState : MenuState
{
    private bool chosen;
    private bool restart;

    internal SelectionState()
    {
        this.type = SELECTION;
    }

    public override int checkTransitions()
    {
        if (restart) return START;
        if (MenuController.schemeType == SchemeType.TWO_CONTROLLERS && chosen) return LOADING;
        if (chosen) return SETUP;
        return type;
    }

    public override void enter()
    {
        base.enter();
        chosen = false;
        restart = false;
        MenuController.ConfigureControllers();
    }

    public override void update(float delta)
    {
        restart = MenuState.GetButton("RTRIG") >= 0;
        int a = MenuState.GetButton("A");
        int b = MenuState.GetButton("B");
        int x = MenuState.GetButton("X");
        int y = MenuState.GetButton("Y");

        if (a >= 0)
        {
            MenuController.controllerIds[0] = a;
            MenuController.LockController(0);
        }
        if (b >= 0)
        {
            MenuController.controllerIds[1] = b;
            MenuController.LockController(1);
        }
        if (x >= 0)
        {
            MenuController.controllerIds[2] = x;
            MenuController.LockController(2);
        }
        if (y >= 0)
        {
            MenuController.controllerIds[3] = y;
            MenuController.LockController(3);
        }

        chosen = MenuController.controllerIds[0] >= 0 && MenuController.controllerIds[1] >= 0;
        if (MenuController.schemeType == SchemeType.FOUR_CONTROLLERS)
        {
            chosen = chosen && MenuController.controllerIds[2] > 0 && MenuController.controllerIds[3] > 0;

        } 
    }
}

class SetupState : MenuState
{
    private bool chosenR;
    private bool chosenL;
    private int rState;
    private int lState;
    private bool restart;

    internal SetupState()
    {
        this.type = SETUP;
    }

    public override int checkTransitions()
    {
        if (restart) return START;
        if (chosenR && chosenL) return LOADING;
        return type;
    }

    public override void enter()
    {
        base.enter();
        chosenR = false;
        chosenL = false;
        rState = 0;
        lState = 2;
        restart = false;
    }

    public override void update(float delta)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log(MenuController.controllerIds[0] + "," + MenuController.controllerIds[1] + "," + MenuController.controllerIds[2] + "," + MenuController.controllerIds[3]);
        }
        if (MenuState.GetButton("A") == MenuController.controllerIds[rState])
        {
            chosenR = true;
            MenuController.LockIcons("R", true);
        }
        else if (!chosenR && MenuState.GetButton("B") == MenuController.controllerIds[rState])
        {
            SwitchPlayers("R");
            rState = (rState + 1) % 2;
        }
        else if (MenuState.GetButton("RTRIG") == MenuController.controllerIds[rState])
        {
            if (chosenR)
            {
                chosenR = false;
                MenuController.LockIcons("R", false);
            }
            else
            {
                restart = true;
            }
        }

        if (MenuState.GetButton("X") == MenuController.controllerIds[lState])
        {
            chosenL = true;
            MenuController.LockIcons("L", true);
        }
        else if (!chosenL && MenuState.GetButton("Y") == MenuController.controllerIds[lState])
        {
            SwitchPlayers("L");
            lState = (lState + 1) % 2 + 2;
        }
        else if (MenuState.GetButton("LTRIG") == MenuController.controllerIds[lState])
        {
            chosenL = false;
            MenuController.LockIcons("L", false);
        }
    }

    private void SwitchPlayers(string side)
    {
        if (side.Equals("R"))
        {
            int tmp = MenuController.controllerIds[0];
            MenuController.controllerIds[0] = MenuController.controllerIds[1];
            MenuController.controllerIds[1] = tmp;
        }
        else
        {
            int tmp = MenuController.controllerIds[2];
            MenuController.controllerIds[2] = MenuController.controllerIds[3];
            MenuController.controllerIds[3] = tmp;
        }

        MenuController.SwitchIcons(side);
    }

}

class LoadingState : MenuState
{
    internal LoadingState()
        : base()
    {
        this.type = LOADING;
    }

    public override int checkTransitions()
    {
        return type;
    }

    public override void enter()
    {
        base.enter();
        MenuController.StartGame();
    }
}

abstract class MenuState : FSMState
{
    public static int GetButton(string postfix) {
        for (int i=0; i<4; i++) {
            string bName = "Player " + i + " " + postfix;
            if (Input.GetButtonDown(bName)) {
                Debug.Log("hoi " + bName);
                return i;
            }
        }
        return -1;
    }

    public static short START = 0;
    public static short SCHEME = 1;
    public static short SELECTION = 2;
    public static short SETUP = 3;
    public static short LOADING = 4;

    public MenuState()
        : base()
    {
    }

    public override void enter()
    {
        MenuController.enableMenu(type);
        Debug.Log("entering menu state " + type);
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