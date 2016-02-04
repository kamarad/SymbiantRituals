using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour {

    public GameManager gameManager;
    public UnityEngine.UI.Text text;
    
    // Use this for initialization
	void Start () {
        text.text = "0-0";
	}
	
	// Update is called once per frame
	void Update () {
        string t = gameManager.t1Score + "-" + gameManager.t2Score;
        text.text = t;
	}
}
