using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Components")]
    public AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;



    public void OnEnable()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("MasterVolume") / 20);
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("MusicVolume") / 20);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("SFXVolume") / 20);
        }
    }
    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusicLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSFXLevel(float sliderValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSensitivity(float sliderValue)
    {

    }
}
