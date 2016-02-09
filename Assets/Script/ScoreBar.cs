using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {

    public const float baseWidth = 960f;
    RectTransform maskRect;
    private const float sizeBuffer = 55f;

    public TEAMS team;

    private float scoreLimit;
    
    // Use this for initialization
	void Start () {
        maskRect = GetComponent<RectTransform>();

        SetProgress(team, 0);

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreLimit = gameManager.scoreLimit;
        gameManager.scoreBarHandlers += SetProgress;
    }

    public void SetProgress(TEAMS team, int score)
    {
        if (this.team == team)
        {
            float fullWidth = baseWidth - (2f * sizeBuffer);
            float newWidth = ((fullWidth * (float)score) / (scoreLimit));

            StartCoroutine(TweenWidth(newWidth));
        }
    }

    private void ChangeBar(float newWidth)
    {
        maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth + sizeBuffer);
    }

    IEnumerator TweenWidth(float w)
    {
        int iterations = 30;
        float currentWidth = maskRect.sizeDelta.x;
        float difference = (w - currentWidth) / (float)iterations;
        for (int i = 0; i < iterations; i++)
        {
            float nW = currentWidth + (difference * i);
            ChangeBar(nW);
            yield return null;
        }

        ChangeBar(w);   // Finally set to final
        yield break;
    }
}
