using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class ChangeRemainAction : MonoBehaviour
{
    public TextMeshProUGUI remainAction;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        remainAction.text = DontDestroyObject.Instance.remainAction.ToString();
    }

    private void FixedUpdate()
    {
        
    }


}
