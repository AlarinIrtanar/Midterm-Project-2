using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player")
        {
            audioSource.Play();
            GameManager.Instance.YouWin();
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
