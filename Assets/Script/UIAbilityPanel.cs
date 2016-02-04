using UnityEngine;
using System.Collections;

public class UIAbilityPanel : MonoBehaviour {

    public SymbiotState symbiotState;

    public UnityEngine.UI.Image img_shoot;
    public UnityEngine.UI.Image img_reflect;
    public UnityEngine.UI.Image img_wall;
    public UnityEngine.UI.Image img_dash;
    
    // Use this for initialization
	void Start () {
        
        symbiotState.onCDFire += HandleFireChange;
        symbiotState.onCDReflect += HandleReflectChange;
        symbiotState.onCDDash += HandleDashChange;
        symbiotState.onCDWall += HandleWallChange;
        
    }

    public void HandleFireChange(bool cd)
    {
        img_shoot.enabled = !cd;
    }

    public void HandleReflectChange(bool cd)
    {
        img_reflect.enabled = !cd;
    }

    public void HandleWallChange(bool cd)
    {
        img_wall.enabled = !cd;
    }

    public void HandleDashChange(bool cd)
    {
        img_dash.enabled = !cd;
    }
}
