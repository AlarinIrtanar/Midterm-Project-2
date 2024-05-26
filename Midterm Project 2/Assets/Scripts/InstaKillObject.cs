using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKillObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(9999, this.name);
        }
    }
}
