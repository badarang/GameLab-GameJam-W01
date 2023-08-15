using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
public class GetButtonTextAndOpenScene : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OpenScene()
    {
        SceneManager.LoadScene("Stage" + buttonText.text);
        GameObject obj = GameObject.Find("GameManager");
        obj.GetComponent<GameManager>().curStage = Int32.Parse(buttonText.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
