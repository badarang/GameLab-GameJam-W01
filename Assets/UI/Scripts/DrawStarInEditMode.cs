using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawStarInEditMode : MonoBehaviour
{
    public int star = 3;
    public Image curImage;
    public Sprite[] starImages;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (star == 1)
        {
            curImage.sprite = starImages[0];
        } 
        else if (star == 2)
        {
            curImage.sprite = starImages[1];
        }
        else if (star == 3)
        {
            curImage.sprite = starImages[2];
        }
    }
}
