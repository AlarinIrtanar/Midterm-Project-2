using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamage
{

    [SerializeField] int Health = 5;

    public void TakeDamage(int damage)
    {
        Health -= damage;   
        if(Health <= 0)
        {
            GameManager.Instance.score += 10;
            Destroy(gameObject);
        }
    }
   


   public void DestroyTarget()
   {
        Destroy(gameObject);
   }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
