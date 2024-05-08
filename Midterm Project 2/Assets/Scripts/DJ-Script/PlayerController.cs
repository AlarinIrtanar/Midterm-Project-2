using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{


    [Header("----palyer--------")]
    [SerializeField] CharacterController controller;
    [SerializeField] int speed;
    [SerializeField] int jumpSpeed;
    [SerializeField] int maxJumps;
    [SerializeField] int gravity;
    [SerializeField] int Hp;

    [Header("-----Shooting-----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;


    Vector3 movedir;
    Vector3 playerVel;
    bool isShooting;
    int jumpedTimes;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        Movement();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpedTimes = 0;
            playerVel = Vector3.zero;   
        }

        movedir = (Input.GetAxis ("Horizontal") * transform.right) +
                   (Input.GetAxis("Vertical") * transform.forward );
        controller.Move(movedir * speed * Time.deltaTime);

        if (Input.GetButton("Shoot")&& !isShooting)
        {
            StartCoroutine(shoot());
        }


        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps)
        {
            jumpedTimes++;
            playerVel.y = jumpSpeed;

        }
        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    public void takeDamage(int amount)
    {
        Hp -= amount;

        if(Hp < 0)
        {
            die();
        }

    }

    public void die()
    {
        Destroy(gameObject);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)), out hit,shootDist))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
               dmg.takeDamage(shootDamage);
            }
        }
        
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
