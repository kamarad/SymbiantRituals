using UnityEngine;
using System.Collections;

public class GolemCreateWall : MonoBehaviour {

    public PlayerEnergy playerEnergy;

    public GameObject prefab_golemWall;
    public GameObject prefab_wallParticles;
    public Transform wallSpawn;

    private string golemWallButton;

    private GolemWall currentWall;
    

    void Update()
    {
        //if (Input.GetKeyDown(inputName))  TODO: Swap to actual system!
        if (Input.GetButtonDown(golemWallButton))
        {
            DoWall();
        }
    }

    public void DoWall()
    {
        // check if another wall
        if (currentWall != null) return;    // Can't create two walls at the same time!

        playerEnergy.gameObject.GetComponent<SymbiotState>().WallUp = true;
        transform.parent.gameObject.GetComponent<GolemAudio>().Wall();

        // Create and store wall
        GameObject wall = GameObject.Instantiate(prefab_golemWall, wallSpawn.position, wallSpawn.rotation) as GameObject;
        currentWall = wall.GetComponent<GolemWall>();
        currentWall.onDestroy += ClearWallOnDestroy;
    }

    private void ClearWallOnDestroy()
    {
        currentWall.onDestroy -= ClearWallOnDestroy;    // Clear event
        currentWall = null;
    }

    public void SetInputs(SymbiotInputs inputs)
    {
        golemWallButton = inputs.golemWallButton;
    }
}
