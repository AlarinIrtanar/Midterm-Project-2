using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableWalls : MonoBehaviour
{

    public Transform startpoint;
    public Transform endpoint;
    public float speed = 2f;

    Vector3 targetPosition;



    // Start is called before the first frame update
    void Start()
    {
        targetPosition = startpoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if(targetPosition == startpoint.position)
            {
                targetPosition = endpoint.position;
            }
            else
            {
                targetPosition = startpoint.position;
            }
        }
    }
}
