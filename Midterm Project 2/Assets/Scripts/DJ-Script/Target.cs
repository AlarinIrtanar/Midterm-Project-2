using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour , IDamage
{

    public int Health = 5;

    bool isDestroyed = false;
    

    public void takeDamage(int damage)
    {
        Health -= damage;   
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
   


   public void DestroyTarget()
   {
      
        isDestroyed = true;
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
