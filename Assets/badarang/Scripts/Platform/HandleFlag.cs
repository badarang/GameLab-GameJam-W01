using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleFlag : MonoBehaviour
{
    private bool isPlayed = false;
    public Animator animator;
    public GameObject starCanvas;
    Rigidbody2D rb;
    [SerializeField] private GameObject fireWorkParticle = default;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayed)
        {
            isPlayed = true;
            animator.SetTrigger("Flag");
            StartCoroutine("Particle");
        }
    }
    
    IEnumerator Particle()
    {
        yield return new WaitForSeconds(1.0f);
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 10f));
        Instantiate(starCanvas, spawnPosition, Quaternion.identity);
        createFireWork();
        yield return new WaitForSeconds(1.0f);
        createFireWork();
        createFireWork();
        yield return new WaitForSeconds(1.0f);
        createFireWork();
        createFireWork();
        createFireWork();
        yield return new WaitForSeconds(1.0f);
        DontDestroyObject.gameManager.GoLobby();
        DontDestroyObject.Instance.setClearedStageAndSaveData();
    }

    void createFireWork()
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(0, 1);
        float size = Random.Range(2.0f, 6.0f);
        GameObject particle = Instantiate(fireWorkParticle, rb.transform.position + new Vector3(randomX * size, randomY * size, 0f), transform.rotation);
        ParticleSystem particlesys = particle.GetComponent<ParticleSystem>();
        particlesys.Play();
    }
}
