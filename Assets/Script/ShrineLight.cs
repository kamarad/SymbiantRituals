using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class ShrineLight : MonoBehaviour {
    [SerializeField]
    private float minIntensity = 0.25f;
    [SerializeField]
    private float maxIntensity = 0.5f;

    private float baseIntensity;
    private Light shrineLight;
    private float random;

    public bool flicker;

	// Use this for initialization
	void Start () {
        shrineLight = GetComponent<Light>();
        baseIntensity = shrineLight.intensity;
        random = Random.Range(0.0f, 65535.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (flicker)
        {
            float noise = Mathf.PerlinNoise(random, Time.time * 5);
            GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }

    public void FadeIn()
    {
        flicker = false;
        StartCoroutine(FadeInLight(shrineLight, baseIntensity));
    }

    public static IEnumerator FadeInLight(Light l, float baseIntensity)
    {
        l.enabled = true;
        for (int n = 0; n <= 15; n++)
        {
            l.intensity = (baseIntensity * (float)n) / 15f;
            yield return null;
        }
    }

}
