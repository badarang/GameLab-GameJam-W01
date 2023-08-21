using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPanel : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickReset()
    {
        DontDestroyObject.Instance.SaveDataToDefault();
        DontDestroyObject.Instance.InitInit();
    }
    public void OnClickCheat()
    {
        DontDestroyObject.Instance.StageCheat();
        DontDestroyObject.Instance.InitInit();
    }
    
    public void OnClickDestroy()
    {
        anim.SetTrigger("Destroy");
        StartCoroutine("Destroy");
    }

    IEnumerator Destroy()
    {
        GameObject transparentPanel = GameObject.Find("TransParentBackGroundPanel(Clone)");
        if (transparentPanel != null)
        {
            // GameObject 파괴
            Destroy(transparentPanel);
        }
        yield return new WaitForSeconds(.8f);
        Destroy(gameObject);
    }
}
