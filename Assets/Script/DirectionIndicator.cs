using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectionIndicator : MonoBehaviour {

    public RectTransform baseCircle;
    public RectTransform altarBlip;

    public GameObject prefab_shrineIndicator;
    
    public Transform player;
    public Transform altar;
    public ActivateShrines activateShrines;

    Dictionary<Shrine, RectTransform> activeIndicators;
    float circleRadius;
    
    void Start()
    {
        activeIndicators = new Dictionary<Shrine, RectTransform>();

        // Get initial circle radius from baseCircle
        circleRadius = baseCircle.sizeDelta.x / 2f;

        activateShrines.onShrinePowerUp += HandleShrinePowerUp;
        activateShrines.onShrinePowerDown += HandleShrinePowerDown;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the positions of the shrine indicators first
        RefreshShrineIndicators();

        // If the player has powers, activate and update the Altar indicator
        if (player.GetComponent<PlayerEnergy>().hasPowers)
        {
            altarBlip.gameObject.SetActive(true);
            SetBlipLocation(player, altar, altarBlip);
        } else
        {
            altarBlip.gameObject.SetActive(false);
        }
    }
    
    void RefreshShrineIndicators()
    {
        foreach (KeyValuePair<Shrine, RectTransform> kvpair in activeIndicators)
        {
            SetImageRotation(player, kvpair.Key.transform, kvpair.Value);
        }
    }

    void SetImageRotation(Transform fromObject, Transform toObject, RectTransform imgToRotate)
    {
        Vector3 direction = toObject.position - fromObject.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(-direction.x, direction.z);
        imgToRotate.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Only used for temple blip, basically
    void SetBlipLocation(Transform fromObject, Transform toObject, RectTransform blipImage)
    {
        Vector3 direction = toObject.position - fromObject.position;
        float angle = Mathf.Atan2(-direction.x, direction.z);
        
        blipImage.localPosition = new Vector3(
            -circleRadius * Mathf.Sin(angle),
            circleRadius * Mathf.Cos(angle),
            0f
            );
    }

    #region Event Handlers
    void HandleShrinePowerUp(Shrine shrine)
    {
        if (!activeIndicators.ContainsKey(shrine))
        {
            // Create new indicator and make child of self
            RectTransform newIndicator = GameObject.Instantiate(prefab_shrineIndicator).GetComponent<RectTransform>();
            newIndicator.SetParent(this.transform);
            newIndicator.position = baseCircle.position;
            newIndicator.localScale = new Vector3(1f, 1f, 1f);

            // Add to activeIndicators Dict
            activeIndicators.Add(shrine, newIndicator);
        }
        
        // Do a refresh pass to display correctly before next Update()
        RefreshShrineIndicators();
    }

    void HandleShrinePowerDown(Shrine shrine)
    {
        if (shrine == null) return;
        // Get indicator if it exists
        RectTransform indicator;
        if (activeIndicators.TryGetValue(shrine, out indicator))
        {
            // Destroy and remove from Dict so it won't get updated anymore
            Destroy(indicator.gameObject);
            activeIndicators.Remove(shrine);
        }
    }

    #endregion
}
