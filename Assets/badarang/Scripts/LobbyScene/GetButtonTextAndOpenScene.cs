using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class GetButtonTextAndOpenScene : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public bool isOpen = false;
    public Image starImage;
    public Sprite[] starImages;
    private Color lockedColor;
    private Color openedColor;
    private bool buttonClicked;

    private int collectedStar;
    private string curStageText;
    
    // Start is called before the first frame update
    void Start()
    {
        curStageText = buttonText.text;
        buttonClicked = false;
        lockedColor = new Color(.44f, .44f, .44f, 1f);
        openedColor = new Color(1f, 1f, 1f, 1f);
    }
    
    public void OpenScene()
    {
        buttonClicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonClicked && isOpen)
        {
            buttonClicked = false;
            DontDestroyObject.Instance.LoadScene(Int32.Parse(buttonText.text));
        }

        buttonText.text = (DontDestroyObject.Instance.curStageWindow * 5 + int.Parse(curStageText)).ToString();
        
        int curStageNum = Int32.Parse(buttonText.text);
        if (DontDestroyObject.Instance.getClearedStage() + 1 >= curStageNum)
        {
            isOpen = true;
            this.GetComponent<Image>().color = openedColor;
        }
        else
        {
            this.GetComponent<Image>().color = lockedColor;
        }
        switch (DontDestroyObject.Instance.collectedStars[curStageNum - 1])
        {
            case 0:
                starImage.sprite = starImages[0];
                break;
            case 1:
                starImage.sprite = starImages[1];
                break;
            case 2:
                starImage.sprite = starImages[2];
                break;
            case 3:
                starImage.sprite = starImages[3];
                break;
            default:
                break;
        }
    }
}
