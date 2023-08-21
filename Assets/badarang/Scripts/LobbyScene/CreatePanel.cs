using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatePanel : MonoBehaviour
{
    public GameObject panelPrefab;

    public GameObject TransparentpanelPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCreatePanel()
    {
        GameObject transparentPanelInstance = Instantiate(TransparentpanelPrefab, transform);
        // UI Panel 프리팹을 인스턴스화하여 Canvas의 자식으로 생성
        GameObject panelInstance = Instantiate(panelPrefab, transform);

        // UI Panel의 RectTransform을 가져옴
        RectTransform panelRectTransform = panelInstance.GetComponent<RectTransform>();

        // Canvas의 RectTransform을 가져옴
        RectTransform canvasRectTransform = GetComponent<RectTransform>();

        // Canvas의 크기
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // UI Panel의 크기
        Vector2 panelSize = panelRectTransform.sizeDelta;

        // UI Panel의 위치 설정 (화면 정 가운데)
        Vector2 centeredPosition = Vector2.zero;
        panelRectTransform.anchoredPosition = centeredPosition;
    }
}
