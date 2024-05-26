using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Target : MonoBehaviour, IDamage
{

    [SerializeField] int Health = 5;
    public AudioClip shatterSound;
    public AudioMixerGroup sfxMixer;
    public int cubesPerAxis = 8;
    public float force = 300f;
    public float radius = 2f;

    public void TakeDamage(int damage)
    {
        Health -= damage;   
        if(Health <= 0)
        {
            GameManager.Instance.score += 10;
            DestroyTarget();
        }
    }
    public void TakeDamage(int damage, string deathItemName)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GameManager.Instance.score += 10;
            DestroyTarget();
        }
    }



    void DestroyTarget()
   {
        Vector3 originalScale = transform.localScale;
        Quaternion originalRotation = transform.rotation;
        Vector3 originalPosition = transform.position;

        for (int x = 0; x < cubesPerAxis; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int z = 0; z < cubesPerAxis; z++)
                {
                    CreateCube(new Vector3(x, y, z), originalScale, originalRotation, originalPosition);
                }
            }
        }
        Destroy(gameObject);
    }

    void CreateCube(Vector3 coordinates, Vector3 originalScale, Quaternion originalRotation, Vector3 originalPosition)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = GetComponent<Renderer>().material;

        cube.transform.localScale = new Vector3(
            originalScale.x / cubesPerAxis,
            originalScale.y,
            originalScale.z / cubesPerAxis
        );

        Vector3 firstCubeLocalPosition = new Vector3(
            coordinates.x * cube.transform.localScale.x,
            coordinates.y * cube.transform.localScale.y,
            coordinates.z * cube.transform.localScale.z
        ) - originalScale / 2 + cube.transform.localScale / 2;

        Vector3 firstCubeWorldPosition = originalPosition + originalRotation * firstCubeLocalPosition;
        cube.transform.position = firstCubeWorldPosition;
        cube.transform.rotation = originalRotation;

        if (coordinates == Vector3.zero)
        {
            AudioSource newAudioSource = cube.AddComponent<AudioSource>();
            newAudioSource.clip = shatterSound;
            newAudioSource.volume = 0.25f;
            newAudioSource.outputAudioMixerGroup = sfxMixer;
            newAudioSource.playOnAwake = false;
            newAudioSource.spatialBlend = 1;
            newAudioSource.rolloffMode = AudioRolloffMode.Linear;
            newAudioSource.maxDistance = 45;
            newAudioSource.PlayOneShot(shatterSound);
          
        }

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(force, transform.position, radius);
        Destroy(cube, 7f);
    }
}
