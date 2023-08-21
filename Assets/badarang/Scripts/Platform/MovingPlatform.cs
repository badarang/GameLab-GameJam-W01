using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    public float speed;

    public int startingPoint;

    public Transform[] points;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = (int)(Random.Range(0, 1) * points.Length);
        transform.position = points[startingPoint].position;
        //points[1].transform.position = new Vector3(width, 0, 0);
        //points[2].transform.position = new Vector3(width, height, 0);
        //points[3].transform.position = new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < .02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}
