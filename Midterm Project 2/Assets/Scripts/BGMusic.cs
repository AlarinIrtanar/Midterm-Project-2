using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] List<AudioClip> backgroudMusic;
    [SerializeField] AudioSource audioSource;


    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(backgroudMusic[Random.Range(0, backgroudMusic.Count)]);
        }
    }
}
