using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private GameObject selectionUI;
    private PlayManager playManager;
    private GridSelector gridSelector;

    public PlayMode playMode;
    public SceneLoadState loadState;

    public void StartStage(int stageNum)
    {
        gridSelector = Object.FindObjectOfType<GridSelector>();
        gridSelector.InitSelectionUI(stageNum);
        playMode = PlayMode.EDIT;
    }

    public void EditMode()
    {
        playManager = Object.FindObjectOfType<PlayManager>();
        playManager.editMode = true;
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

    private IEnumerator LoadSceneCo(string name)
    {
        loadState = SceneLoadState.LOAD;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        loadState = SceneLoadState.ENDLOAD;
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