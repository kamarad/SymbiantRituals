using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private float minIntensity = 0.25f;
    [SerializeField]
    private float maxIntensity = 0.5f;

	private float random;
    public bool enabled;

	void Start()
	{
		random = Random.Range(0.0f, 65535.0f);
	}

	void Update()
	{
        if (enabled) {
		    float noise = Mathf.PerlinNoise(random, Time.time*5);
		    GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
	}
}