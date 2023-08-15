using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{

    public GameObject projectile;

    public Transform spawnLocation;

    public Quaternion spawnRotation;

    public float spawnTime = 2.3f;

    private float timeSinceSpawned = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawned += Time.deltaTime;

        if (timeSinceSpawned >= spawnTime)
        {
            Instantiate(projectile, spawnLocation.position, spawnRotation);
            timeSinceSpawned = 0;
        }
    }
}
