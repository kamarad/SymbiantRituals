using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour {

    UnityEngine.UI.Text text;
    
    // Use this for initialization
	void Start () {
        text = GetComponent<UnityEngine.UI.Text>();
        text.text = "0 - 0";
	}
}
