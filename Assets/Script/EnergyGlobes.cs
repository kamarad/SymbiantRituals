using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyGlobes : MonoBehaviour {

    [SerializeField]
    private float floatRadius = .4f;
    [SerializeField]
    private float angularVelocity = 360;
    [SerializeField]
    private float spinToOrbitRatio = 2.5f;
    [SerializeField]
    private GameObject globePrefab;

    private float stepAngle;
    private bool carrying;
    private List<GameObject> globeList = new List<GameObject>();
    private Transform spawner;

	// Use this for initialization
	void Start () {
        spawner = transform.Find("GlobeSpawner");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Activate(3);
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            Deactivate();
        }

        if (carrying)
        {
            spawner.Rotate(Vector3.up, angularVelocity * Time.deltaTime, Space.Self);
            foreach (GameObject globe in globeList)
            {
                globe.transform.Rotate(Vector3.up, angularVelocity * spinToOrbitRatio * Time.deltaTime, Space.Self);
            }
        }
	}

    public void Activate(int numOfSouls)
    {
        carrying = true;
        stepAngle = (Mathf.PI*2) / numOfSouls;


        updateList(numOfSouls);
        placeGlobes();
    }

    public void Deactivate()
    {
        carrying = false;
        foreach (GameObject globe in globeList)
        {
            globe.SetActive(false);
        }
    }

    private void placeGlobes()
    {
        int i = 0;
        foreach (GameObject globe in globeList)
        {
            Vector3 pos = new Vector3(Mathf.Cos(stepAngle * i), 0, Mathf.Sin(stepAngle * i));
            Debug.Log(pos);
            globe.transform.localPosition = pos * floatRadius;
            globe.transform.Rotate(Vector3.up, (360/(Mathf.PI*2))*stepAngle * i, Space.Self);
            globe.SetActive(true);
            i++;
        }
    }

    private void updateList(int count)
    {
        int size = globeList.Count;
        if (size < count)
        {
            for (int i = 0; i < (count - size); i++)
            {
                GameObject globe = GameObject.Instantiate<GameObject>(globePrefab);
                globe.transform.position = spawner.position;
                globe.transform.parent = spawner;
                globe.SetActive(true);
                globeList.Add(globe);
            }
        }
        else if (size > count)
        {
            for (int i = 0; i < (size - count); i++)
            {
                globeList.RemoveAt(0);
            }
        }
    }
}
