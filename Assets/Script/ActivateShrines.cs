using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateShrines : MonoBehaviour {

	public int maxPoweredShrines = 2;
	public float minimumShrineInterval = 5f;
	public float maximumShrineInterval = 15f;
	public Shrine[] shrines;

	private float timer;
	private float interval;

	// Use this for initialization
	void Start () {
		NewCycle();
	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if (timer >= interval)
		{
			int active = 0;
			foreach (Shrine shrine in shrines)
			{
				if (shrine.hasPower) active++;
			}

			if (active < maxPoweredShrines)
			{
				DoPower();
			}

			// Restart the timer
			NewCycle();
		}
	}

	public void NewCycle()
	{
		timer = 0f;
		interval = Mathf.Lerp(minimumShrineInterval, maximumShrineInterval, Random.Range(0f, 1f));
	}

	private void DoPower()
	{
		int index = Mathf.FloorToInt(Random.Range(0f, (float)(shrines.Length)));
		if (shrines[index].IsReadyForPower())
		{
			shrines[index].GivePower();
            Debug.Log("Shrine " + shrines[index].gameObject.name + " has power");
		}
	}
}
