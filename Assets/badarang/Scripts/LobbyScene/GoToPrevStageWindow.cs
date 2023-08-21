using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToPrevStageWindow : MonoBehaviour
{
    void Update()
    {
        if (DontDestroyObject.Instance.curStageWindow == 0)
        {
            Color col = GetComponent<Image>().color;
            col.a = 0f;
            GetComponent<Image>().color = col;
        }
        else
        {
            Color col = GetComponent<Image>().color;
            col.a = 1f;
            GetComponent<Image>().color = col;
        }
    }

    public void OnClick()
    {
        if (GetComponent<Image>().color.a == 1f)
        {
            DontDestroyObject.Instance.curStageWindow--;
        }
    }
}
