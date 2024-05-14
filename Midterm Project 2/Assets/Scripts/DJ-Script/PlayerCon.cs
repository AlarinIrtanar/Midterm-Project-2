using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{

    [Header("------components-------")]
    [SerializeField] CharacterController controller;


    [Header("----player--------")]

    [SerializeField] int hp;
    [SerializeField] int speed;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpTimes;
    [SerializeField] int maxJumps;
    [SerializeField] int gravity;

    [Header("----Shooting--------")]

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;


    Vector3 moveDir;    
    Vector3 playerVel;
    bool isShooting;
    int jumpedTimes;
    int hPOrig;
    


    // Start is called before the first frame update
    void Start()
    {
        hPOrig = hp;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpedTimes = 0;    
            playerVel = Vector3.zero;
        }
        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                 (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);

        if(Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps)
        {
            jumpedTimes++;
            playerVel.y = jumpSpeed;
        }
        playerVel.y -= gravity * Time.deltaTime; 
        controller.Move(playerVel * Time.deltaTime);

    }

    public void TakeDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            GameManager.Instance.YouLose();
        }
    }



    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (hit.transform != transform && dmg != null)

            {
                dmg.TakeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }

  
}
