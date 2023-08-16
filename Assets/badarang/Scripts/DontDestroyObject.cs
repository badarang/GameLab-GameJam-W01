using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    public static DontDestroyObject instance = null;
    // Start is called before the first frame update

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
        }
        
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<DontDestroyObject>();
    }
}
