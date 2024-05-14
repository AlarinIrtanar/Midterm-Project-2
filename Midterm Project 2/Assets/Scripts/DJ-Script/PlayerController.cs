using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{


    [Header("----player--------")]
  
    [SerializeField] int Hp;
    [SerializeField] AudioSource gunshotAS;



    int HpOrig;
    bool isShooting;
    float shootRate = .5f;

    // Start is called before the first frame update
    void Start()
    {
        HpOrig = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Shoot") && !isShooting && !MenuManager.instance.isPaused)
        {
            StartCoroutine(shoot());
        }
    }

   

    public void TakeDamage(int amount)
    {
        Hp -= amount;

        if(Hp <= 0)
        {
            die();
        }

    }

    public void die()
    {
        GameManager.Instance.YouLose();
    }

    IEnumerator shoot()
    {
        isShooting = true;

        gunshotAS.Play();
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }





}
