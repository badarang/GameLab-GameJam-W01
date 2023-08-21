using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScreenManager : MonoBehaviour
{
    [SerializeField] private Star[] Stars;

    [SerializeField] private float EnlargeScale = 1.5f;

    [SerializeField] private float ShrinkScale = 1f;

    [SerializeField] private float EnlargeDuration = .25f;

    [SerializeField] private float ShrinkDuration = .25f;
    // Start is called before the first frame update
    void Start()
    {
        ShowStars(DontDestroyObject.Instance.star);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowStars(int numOfStars)
    {
        StartCoroutine(ShowStarsRoutine(numOfStars));
    }

    private IEnumerator ShowStarsRoutine(int numOfStars)
    {
        foreach (Star star in Stars)
        {
            star.YellowStar.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < numOfStars; i++)
        {
            yield return StartCoroutine(EnlargeAndShrinkStar(Stars[i]));
        }
    }

    private IEnumerator EnlargeAndShrinkStar(Star star)
    {
        yield return StartCoroutine(ChangeStarScale(star, EnlargeScale, EnlargeDuration));
        yield return StartCoroutine(ChangeStarScale(star, ShrinkScale, ShrinkDuration));
    }

    private IEnumerator ChangeStarScale(Star star, float targetScale, float duration)
    {
        Vector3 initialScale = star.YellowStar.transform.localScale;
        Vector3 finalScale = new Vector3(targetScale, targetScale, targetScale);
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            star.YellowStar.transform.localScale = Vector3.Lerp(initialScale, finalScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        star.YellowStar.transform.localScale = finalScale;
    }
}
