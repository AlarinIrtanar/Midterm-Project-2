using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour 
{
    [SerializeField] int damageAmount;
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damageAmount, this.name);
        }
    }

    
}
