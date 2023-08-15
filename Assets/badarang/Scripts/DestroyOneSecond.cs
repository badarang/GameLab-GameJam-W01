using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOneSecond : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator runCoroutine = RunCoroutine ();
        while (runCoroutine.MoveNext ()) {
            object result = runCoroutine.Current;

            if (result is WaitForSeconds) {
                // 원하는 초만큼 기다리는 로직 수행
                // 여기 예제에서는 1초만큼 기다리게 될 것임을 알 수 있음
            }
            else if ...
        }
    }
    
    IEnumerator RunCoroutine() {
        yield return new WaitForSeconds(1.0f);
    }
}
