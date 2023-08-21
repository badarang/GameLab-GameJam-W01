using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    public Image YellowStar;
    // Start is called before the first frame update
    void Awake()
    {
        YellowStar = GetComponent<Image>();
        YellowStar.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
