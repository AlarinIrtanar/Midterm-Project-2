using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderOutofBounds : MonoBehaviour
{
    // Start is called before the first frame update
   public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.YouLose();

        }

    }




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
