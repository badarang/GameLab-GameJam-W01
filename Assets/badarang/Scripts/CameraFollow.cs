using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
private Vector3 offset = new Vector3(0f, 0f, -10f);
private float smoothTime = .25f;
private Vector3 velocity = Vector3.zero;
public float ShakeAmount;

private float ShakeTime;
private float flagDelay = 1.3f;

[SerializeField] private Transform target;
[SerializeField] private Transform targetFlag;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SetCameraToFollowPlayer");
    }

    IEnumerator SetCameraToFollowPlayer()
    {
        yield return new WaitForSeconds(.9f);
        transform.position = targetFlag.position;
        yield return new WaitForSeconds(1.1f);
        if (DontDestroyObject.gameManager.curStage != 1)
            DontDestroyObject.gameManager.EditMode();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (flagDelay > 0) flagDelay -= Time.deltaTime;
        if (flagDelay > 0)
        {
            transform.position = targetFlag.position;
        }
        else
        {
            if (target != null)
            {
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        
            if (ShakeTime > 0)
            {
                transform.localPosition += Random.insideUnitSphere * ShakeAmount;
                ShakeTime -= Time.deltaTime;
            }
            else
            {
                ShakeTime = 0.0f;
            }
        }
    }
	
    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }
}
