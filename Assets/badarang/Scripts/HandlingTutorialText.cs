using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandlingTutorialText : MonoBehaviour
{
    public string displayNum;
    // Start is called before the first frame update
    public TextMeshProUGUI textMeshPro;
    public Image child1, child2, child3;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (DontDestroyObject.Instance.IsEditMode())
        {
            if (displayNum == GameObject.Find("ScoreAnimator").GetComponent<ChangeScore>().newScore.text)
            {
                SetAlphaRecursively(transform, 1f);
            }
            else
            {
                SetAlphaRecursively(transform, 0f);
            }
        }
        
        if (displayNum == GameObject.Find("ScoreAnimator").GetComponent<ChangeScore>().newScore.text)
        {
            SetAlphaRecursively(transform, 1f);
        }
        else
        {
            SetAlphaRecursively(transform, 0f);
        }
    }
    
    private void SetAlphaRecursively(Transform target, float alphaValue)
    {
        Color color = child1.color;
        color.a = alphaValue;
        textMeshPro.color = color;
        child1.color = color;
        child2.color = color;
        child3.color = color;
    }
}
