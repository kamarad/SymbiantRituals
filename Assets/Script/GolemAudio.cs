using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource), typeof(AudioSource), typeof(Animator))]
public class GolemAudio : MonoBehaviour {

    private AudioSource walkAudioBig;
    private AudioSource abilityAudio;

    [SerializeField]
    private AudioClip dash;
    [SerializeField]
    private AudioClip wall;

    private Animator animator;

    private float roarMinTime = 3;
    private float roarMaxTime = 8;
    private float roarTime = 0;

    // Use this for initialization
	void Start () {
        AudioSource[] list = GetComponents<AudioSource>();
        walkAudioBig = list[0];
        abilityAudio = list[1];
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animator.GetBool("walk") && !walkAudioBig.isPlaying)
        {
            walkAudioBig.Play();
        } else if (!animator.GetBool("walk")) {
            walkAudioBig.Stop();
        }

        if (roarTime > 0)
        {
            roarTime -= Time.deltaTime;
        }
        else
        {
            roarTime = 0.3f * (roarMaxTime - roarMinTime) + roarMinTime;
    //        roar.Play();
        }

    }

    public void Dash()
    {
        if (dash != null)
        {
            abilityAudio.PlayOneShot(dash);
        }
    }

    public void Wall()
    {
        if (wall != null)
        {
            abilityAudio.PlayOneShot(wall);
        }
    }
}
