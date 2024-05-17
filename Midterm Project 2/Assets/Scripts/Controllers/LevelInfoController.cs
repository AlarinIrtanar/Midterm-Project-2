using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInfoController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_Text levelNumText;
    [SerializeField] TMP_Text levelNameText;
    [SerializeField] Image lockedImage;

    [Header("Audio")]
    [SerializeField] AudioSource selectAud;
    [SerializeField] AudioSource unlockAud;

    [Header("Animations")]
    [SerializeField] Animator unlockAnim;

    [Header("Effects")]
    [SerializeField] ParticleSystem unlockedEffect;

    bool isUnlocked;
    SelectableWorlds parent;

    private void OnEnable()
    {
        unlockedEffect.Pause();
    }
    private void OnDisable()
    {
        unlockedEffect.gameObject.SetActive(false);
    }
    public void SetParent(SelectableWorlds parent)
    {
        this.parent = parent;
    }
    public void SetNumber(int levelNumber)
    {
        levelNumText.text = levelNumber.ToString();
    }
    public void SetName(string levelName)
    {
        levelNameText.text = levelName;
    }
    public void UnlockAnimation()
    {
        unlockedEffect.gameObject.SetActive(true);
        unlockAnim.enabled = true;
        unlockedEffect.Play();
        unlockAud.Play();
    }
    public void SetUnlocked(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
        if (isUnlocked)
        {
            lockedImage.gameObject.SetActive(false);
        }
        else
        {
            lockedImage.gameObject.SetActive(true);
        }
    }
    public bool GetUnlocked()
    {
        return isUnlocked;
    }
    public void levelSelected()
    {
        selectAud.Play();
        if (isUnlocked)
        {
            //Debug.Log("Attempting to go to: " +  levelNameText.text); // Comment when levels are created
            PlayerPrefs.SetInt("SelectedLevel", int.Parse(levelNumText.text));
            SceneManager.LoadScene(levelNameText.text); // Uncomment when levels are created
        }
    }
}
