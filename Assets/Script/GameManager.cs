using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TEAMS
{
    ONE = 0,
    TWO
}


public class GameManager : MonoBehaviour {

    public delegate void ScoreEvent(TEAMS team, int score);
    public ScoreEvent scoreBarHandlers;

    [SerializeField]
    private Text scoreOne;
    [SerializeField]
    private Text scoreTwo;

    public int t1Score
    {
        get
        {
            return _t1Score;
        }
    }
    public int t2Score
    {
        get
        {
            return _t2Score;
        }
    }
    
    private int _t2Score;
	private int _t1Score;
	private float timer = 0f;
    
    [SerializeField]
    public int scoreLimit = 3;
    [SerializeField]
    private float maxRoundLength = 180f; // 3 minutes

    private Vector3[] startPositions;
    private Quaternion[] startRotations;

	public void AddScore(TEAMS team, int amount)
	{
		if (team == TEAMS.ONE)
		{
			_t1Score += amount;
			// Update UI!
		} else
		{
			_t2Score += amount;
			// Update UI!
		}
        if (scoreBarHandlers != null)
        {
            scoreBarHandlers(TEAMS.ONE, _t1Score);
            scoreBarHandlers(TEAMS.TWO, _t2Score);
        }

        Debug.Log("Score changed");
        scoreOne.text = t1Score + " - " + t2Score;
        scoreTwo.text = t2Score + " - " + t1Score;
        CheckVictory();


	}

	void CheckVictory()
	{
        if (_t1Score >= scoreLimit || _t2Score >= scoreLimit)
        {
            if (_t1Score == _t2Score)
            {
                // TODO: Draw! :(
            }
            else
            {
                Win(_t1Score > _t2Score);
            }
        }
	}

	// Use this for initialization
	void Start () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        startPositions = new Vector3[players.Length];
        startRotations = new Quaternion[players.Length];

        for (int i=0; i<players.Length; i++) {
            startPositions[i] = players[i].transform.position;
            startRotations[i] = players[i].transform.rotation;
        }
        ResetGame();
	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if (timer >= maxRoundLength)
		{
			CheckVictory();
		}

        //test code
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Win(true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Win(false);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ResetGame();
        }
    }

	void Win(bool teamOne)
	{
        if (teamOne)
        {
            Debug.LogError("Team One wins!");
            GetComponent<VictoryEffect>().DoEffect(TEAMS.ONE);
        }
        else
        {
            Debug.LogError("Team Two wins!");
            GetComponent<VictoryEffect>().DoEffect(TEAMS.TWO);
        }
        GetComponentInChildren<Renderer>().enabled = true;
    }

    public void ResetGame()
    {
        _t1Score = 0;
        _t2Score = 0;
        timer = 0f;
        GetComponent<VictoryEffect>().Reset();
        GetComponentInChildren<Renderer>().enabled = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = startPositions[i];
            players[i].transform.rotation = startRotations[i];
            players[i].GetComponent<PlayerEnergy>().Reset();
        }
    }

}
