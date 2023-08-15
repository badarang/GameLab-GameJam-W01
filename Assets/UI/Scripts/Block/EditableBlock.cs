using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class EditableBlock : BlockBase, IInstallable, IDeletable
{
    private SpriteRenderer sr;

    private bool deletable;
    private Action<GameObject> onDeleteCallback;
    private Action<GameObject> onHoverCallback;
    private Action<GameObject> onExitHoverCallback;

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

    public void SetOnHoverCallback(Action<GameObject> callback)
    {
        onHoverCallback = callback;
    }

    public void SetOnExitHoverCallback(Action<GameObject> callback)
    {
        onExitHoverCallback = callback;
    }

    public void onClicked()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && deletable)
        {
            onDeleteCallback(this.gameObject);
        }
    }

    public void onHovered()
    {
        if (deletable)
            onHoverCallback(this.gameObject);
    }

    public void onExited()
    {
        onExitHoverCallback(this.gameObject);
    }

    public void OnMouseDown()
    {
        Debug.Log("click");
        onClicked();
    }

    public void OnMouseOver()
    {
        Debug.Log("hover");
        onHovered();
    }

    public void OnMouseExit()
    {
        Debug.Log("exit");
        onExited();
    }
}
