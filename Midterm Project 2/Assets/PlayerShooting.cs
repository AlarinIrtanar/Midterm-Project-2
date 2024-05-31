using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootDistance;
    [SerializeField] float shootCooldown;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audShoots;
    [SerializeField] Vector2 audShootVolRange;

    string shootButton;
    bool isShooting;

    private void Start()
    {
        // Set Up Shoot Button
        if (PlayerPrefs.HasKey("Shoot Button"))
            shootButton = PlayerPrefs.GetString("Shoot Button");
        else
            shootButton = "mouse 0";
    }

    void Update()
    {
        // Do shooting
        if (!MenuManager.instance.isPaused && !isShooting && Input.GetKeyDown(shootButton))
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        audioSource.PlayOneShot(audShoots[Random.Range(0, audShoots.Length)], Random.Range(audShootVolRange.x, audShootVolRange.y));
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                // Shot something!!!
                dmg.TakeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }
}
