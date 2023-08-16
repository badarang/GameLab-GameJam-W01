using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private GridSelector gridSelector;

    public PlayMode playMode;
    public SceneLoadState loadState;

    private CameraFollow playCamera;
    private UICameraControl editCamera;
    private GameObject selectionUI;

    public void StartStage(int stageNum)
    {
        gridSelector = UnityEngine.Object.FindObjectOfType<GridSelector>();
        gridSelector.InitSelectionUI(stageNum);
        playMode = PlayMode.EDIT;

        playCamera = UnityEngine.Object.FindObjectOfType<CameraFollow>();
        editCamera = UnityEngine.Object.FindObjectOfType<UICameraControl>();
        selectionUI = GameObject.Find("SelectionUI");
    }

    public void EditMode()
    {
        playMode = PlayMode.EDIT;

        playCamera.enabled = false;
        editCamera.enabled = true;
        editCamera.Init();
        selectionUI.SetActive(true);
    }
    
    public void ExitEditMode()
    {
        playMode = PlayMode.PLAY;

        playCamera.enabled = true;
        editCamera.Init();
        editCamera.enabled = false;
        selectionUI.SetActive(false);
    }

    public void Init()
    {
        playMode = PlayMode.LOBBY;
        loadState = SceneLoadState.ENDLOAD;
    }

    public void GoLobby()
    {
        playMode = PlayMode.LOBBY;
        SceneManager.LoadScene("Lobby");
    }

    public IEnumerator LoadScene(int stageNum)
    {
        loadState = SceneLoadState.LOAD;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Stage" + stageNum, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("asdf");
        yield return new WaitForEndOfFrame();
        loadState = SceneLoadState.ENDLOAD;
        StartStage(stageNum);
    }
}

public enum PlayMode
{
    LOBBY,
    PLAY,
    EDIT
}

public enum SceneLoadState
{
    LOAD,
    ENDLOAD
}