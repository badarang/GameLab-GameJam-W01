using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public CameraFollow playCamera;
    public UICameraControl editCamera;

    public bool editMode = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!editMode)
        {
            playCamera.enabled = true;
            editCamera.enabled = false;
        } else
        {
            playCamera.enabled = false;
            editCamera.enabled = true;
        }
    }
}
