using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("Health")]
  
    [SerializeField] int Hp;
    int HpOrig;

    // Start is called before the first frame update
    void Start()
    {
        HpOrig = Hp;
    }

    public void TakeDamage(int amount)
    {
        Hp -= amount;
        if (Hp <= 0)
            Die();
    }

    public void Die()
    {
        GameManager.Instance.YouLose();
    }
}
