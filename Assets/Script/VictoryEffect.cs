using UnityEngine;
using System.Collections;

public class VictoryEffect : MonoBehaviour {

    [SerializeField]
    private GameObject altarFire;

    [SerializeField]
    private AudioSource victoryMusic;
    [SerializeField]
    private AudioSource bang;

    private bool triggered;
	// Use this for initialization
	void Start () {
	
	}

    public void DoEffect(TEAMS team)
    {
        if (!triggered)
        {
            altarFire.transform.localScale += Vector3.one;
            switch (team)
            {
                case TEAMS.ONE: altarFire.GetComponent<ParticleSystem>().startColor = Color.green; break;
                case TEAMS.TWO: altarFire.GetComponent<ParticleSystem>().startColor = Color.red; break;
                default: altarFire.GetComponent<ParticleSystem>().startColor = Color.white; break;
            }
            triggered = true;

            GetComponent<AudioSource>().Stop();
            bang.Play();
            victoryMusic.Play();
        }
    }

    public void Reset()
    {
        if (triggered)
        {
            altarFire.transform.localScale -= Vector3.one;
            altarFire.GetComponent<ParticleSystem>().startColor = Color.white;
            triggered = false;
            GetComponent<AudioSource>().Play();
            victoryMusic.Stop();
        }
    }
}
