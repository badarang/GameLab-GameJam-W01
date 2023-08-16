using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{

    public float fallDelay = 0;
    private float destroyDelay = 1f;
    private float startX;
    private float startY;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        startX = rb.position.x;
        startY = rb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        //Destroy(gameObject, destroyDelay);
    }

    void ReturnToOriginalPlace()
    {
        rb.position = new Vector2(startX, startY);
    }
}
