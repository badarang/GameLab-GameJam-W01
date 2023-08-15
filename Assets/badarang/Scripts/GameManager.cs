using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject selectionUI;
    public PlayManager playManager;

    public int curStage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartStage()
    {/*
        GameObject uiObj = Instantiate(selectionUI);
        playManager = uiObj.transform.Find("PlayManager").GetComponent<PlayManager>();
        playManager.playCamera = FindObjectOfType<CameraFollow>();
        playManager.editCamera = FindObjectOfType<UICameraControl>();

        playManager.editMode = false;*/
    }

    public void EditMode()
    {
        Debug.Log("enter edit mode1");
        playManager = FindObjectOfType<PlayManager>();
        playManager.editMode = true;
        Debug.Log("enter edit mode2");
    }
}
