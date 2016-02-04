using UnityEngine;
using System.Collections;

public class ZoomCam : MonoBehaviour {

    private Camera cam;
    public GameObject t1;
    public GameObject t2;

    public float minDist = 5f;
    public float maxDist = 18f;
    public float zoomLerpFactor = 0.005f;
    public float translateLerpFactor = 0.001f;

    public float viewBuffer = 2f;

    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

	// Update is called once per frame
	void Update () {
        cam.transform.position = Vector3.Lerp(
            cam.transform.position,
            centerPoint(),
            translateLerpFactor
        );
        float zoomDistance = Mathf.Clamp(
            ((t1.transform.position - t2.transform.position).magnitude) + viewBuffer,
            minDist,
            maxDist
            );
        // Lerp
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            zoomDistance,
            zoomLerpFactor
            );
	}

    Vector3 centerPoint()
    {
        Vector3 centered = Vector3.Lerp(t1.transform.position, t2.transform.position, 0.5f);
        centered.y = 18f;
        return centered;
    }

    float scale()
    {
        return 18f;
    }
}
