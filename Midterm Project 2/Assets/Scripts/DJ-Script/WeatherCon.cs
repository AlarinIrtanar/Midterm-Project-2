using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeatherCon : MonoBehaviour
{
    [SerializeField] ParticleSystem rainParticleSystem;
    [SerializeField] int maxParticles ;
    [SerializeField] float rateOverTime;



    // Start is called before the first frame update
    void Start()
    {
        if (rainParticleSystem == null)
        {
           rainParticleSystem = GetComponent<ParticleSystem>();

        }
        var mainModule = rainParticleSystem.main;
        mainModule.maxParticles = maxParticles;

        var EmissionCon = rainParticleSystem.emission;
        EmissionCon.rateOverTime = rateOverTime;
    }

    // Update is called once per frame
    void Update()
    {
       var mainModule = rainParticleSystem.main;
        mainModule.maxParticles = maxParticles;

       var EmissionCon = rainParticleSystem.emission;
        EmissionCon.rateOverTime = rateOverTime;
    }

    
}
