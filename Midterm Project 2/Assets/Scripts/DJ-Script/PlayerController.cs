using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{


    [Header("----player--------")]
  
    [SerializeField] int Hp;


   
    int HpOrig;

    // Start is called before the first frame update
    void Start()
    {
        HpOrig = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void TakeDamage(int amount)
    {
        Hp -= amount;

        if(Hp <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        GameManager.Instance.Respawn(gameObject);
    }



   

}
