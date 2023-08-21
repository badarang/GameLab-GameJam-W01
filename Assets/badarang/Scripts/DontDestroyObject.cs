using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    public int totalStage;
    public int curStageWindow;
    public int clearedStage;
    public int[] collectedStars;
    public int remainAction, starCriteria_2, starCriteria_3;
    public int star;
    public int[,] starCriterias = new int[,] {
        { 0, 0 },
        { 1, 2 },
        { 3, 5 },
        { 2, 4 },
        { 2, 4 },
        { 3, 5 },
        { 2, 3 },
        { 1, 2 },
        { 2, 4 },
        { 3, 6 }
    };
    
    
    private static DontDestroyObject instance = null;
    public static DontDestroyObject Instance
    {
        get
        {
            if(instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitOtherManagers();
        shoudSceneLoad = false;
        InitInit();
    }

    public void InitInit()
    {
        if (PlayerPrefs.HasKey("clearedStage"))
        {
            clearedStage = PlayerPrefs.GetInt("clearedStage");
        }
        else
        {
            clearedStage = 0;
        }

        if (PlayerPrefs.HasKey("collectedStars"))
        {
            string[] dataArr = PlayerPrefs.GetString("collectedStars").Split(',');
            for (int i = 0; i < dataArr.Length; i++)
            {
                collectedStars[i] = System.Convert.ToInt32(dataArr[i]);
            }
        }
        else
        {
            for (int i = 0; i < totalStage; i++)
            {
                collectedStars[i] = 0;
            }
        }

        if (PlayerPrefs.HasKey("curStageWindow"))
        {
            curStageWindow = PlayerPrefs.GetInt("curStageWindow");
        }
        else
        {
            curStageWindow = 0;
        }
    }
    
    private void InitOtherManagers()
    {
        _gameManager.Init();
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<DontDestroyObject>();
    }

    private static GameManager _gameManager = new GameManager();
    public static GameManager gameManager
    {
        get
        {
            return _gameManager;
        }
    }

    public void SaveDataToDefault()
    {
        PlayerPrefs.DeleteAll();
    }
    
    public void StageCheat()
    {
        PlayerPrefs.SetInt("clearedStage", totalStage - 1);
    }
    public bool IsEditMode()
    {
        return _gameManager.playMode == PlayMode.EDIT;
    }

    public int getCurStage()
    {
        return _gameManager.getCurStage();
    }
    
    public int getClearedStage()
    {
        return clearedStage;
    }
    
    public void SaveData()
    {
        PlayerPrefs.SetInt("clearedStage", clearedStage);
        string strArr = ""; 
        for (int i = 0; i < collectedStars.Length; i++) 
        {
            strArr = strArr + collectedStars[i];
            if (i < collectedStars.Length - 1)
            {
                strArr = strArr + ",";
            }
        }
        PlayerPrefs.SetString("collectedStars", strArr);
        PlayerPrefs.SetInt("curStageWindow", curStageWindow);
        PlayerPrefs.Save();
    }
    public void GoLobby()
    {
        _gameManager.GoLobby();
    }

    public void setClearedStageAndSaveData()
    {
        clearedStage = Mathf.Max(clearedStage, getCurStage());
        collectedStars[getCurStage() - 1] = Mathf.Max(collectedStars[getCurStage() - 1], star);
        SaveData();
    }
    
    public void LoadScene(int stageNum)
    {
        targetStage = stageNum;
        shoudSceneLoad = true;
    }

    private bool shoudSceneLoad;
    private int targetStage;

    public void Update()
    {
        if (shoudSceneLoad)
        {
            shoudSceneLoad = false;
            StartCoroutine(_gameManager.LoadScene(targetStage));
        }
        
    }
}
