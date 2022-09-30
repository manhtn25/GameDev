using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject[] points; //using an array to set the start and end point
    private int curPoint; //logic for setting when to switch the moving object

    [SerializeField] private float speed = 3f;

    
    private void Update()
    {
        if (Vector2.Distance(points[curPoint].transform.position, transform.position) < .1f)
        {
            curPoint++;
            if(curPoint >= points.Length)
            {
                curPoint = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[curPoint].transform.position, Time.deltaTime * speed);
        
    }
}
