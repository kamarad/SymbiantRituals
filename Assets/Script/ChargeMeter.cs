using UnityEngine;
using System.Collections;

public class ChargeMeter : MonoBehaviour {

    public PlayerEnergy playerEnergy;
    public Transform barContent;
    float zOffset = 0.01f;

    public Color normalColor;
    public Color burnoutColor;

    // Use this for initialization
	void Start () {
        playerEnergy.onEnergyChange += HandleValueChange;
        playerEnergy.onBurnout += HandleBurnout;
        //Hide();
	}
	

    public void HandleValueChange(float v)  // Between 0 and 2
    {
        v /= 2f;    // Map 0-1
        barContent.localScale = new Vector3(
            v,
            barContent.localScale.y,
            1f
            );
        barContent.localPosition = new Vector3(
            -(1f- v)/2f,
            0f,
            -zOffset
            );
    }

    void Hide()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        barContent.GetComponent<MeshRenderer>().enabled = false;

    }

    void Show()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        barContent.GetComponent<MeshRenderer>().enabled = true;
    }

    void HandleBurnout(bool bo)
    {
        if (bo)
        {
            this.GetComponent<MeshRenderer>().material.color = burnoutColor;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.color = normalColor;
        }
    }
}
