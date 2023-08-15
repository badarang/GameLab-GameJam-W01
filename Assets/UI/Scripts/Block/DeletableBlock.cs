using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeletableBlock : MonoBehaviour, IDeletable
{
    private SpriteRenderer sr;

    private bool deletable;
    private Action<GameObject> onDeleteCallback;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        deletable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDeletable(bool deletable)
    {
        this.deletable = deletable;
    }

    public void SetOnDeleteCallback(Action<GameObject> callback)
    {
        onDeleteCallback = callback;
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            onDeleteCallback(this.gameObject);
        }
    }

    public void OnMouseOver()
    {
        if (deletable)
            sr.color = new Color(1, 0.3f, 0.3f, 0.75f);
    }

    public void OnMouseExit()
    {
        sr.color = new Color(0, 0, 0, 0.75f);
    }
}
