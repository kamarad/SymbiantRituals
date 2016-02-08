using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {

    public const float baseWidth = 960f;
    public GameManager gameManager;
    RectTransform maskRect;
    private const float sizeBuffer = 55f;

    public TEAMS team;
    
    // Use this for initialization
	void Start () {
        maskRect = GetComponent<RectTransform>();

        SetProgress(0);

        if (team == TEAMS.ONE)
        {
            gameManager.onT1ScoreChange += SetProgress;
        } else
        {
            gameManager.onT2ScoreChange += SetProgress;
        }
    }

    public void SetProgress(int score)
    {
        float fullWidth = baseWidth - (2f * sizeBuffer);
        float newWidth = ((fullWidth * (float)score) / (gameManager.scoreLimit));

        StartCoroutine(TweenWidth(newWidth));
    }

    private void ChangeBar(float newWidth)
    {
        maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth + sizeBuffer);
    }

    IEnumerator TweenWidth(float w)
    {
        int iterations = 30;
        float currentWidth = maskRect.sizeDelta.x - sizeBuffer;
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
