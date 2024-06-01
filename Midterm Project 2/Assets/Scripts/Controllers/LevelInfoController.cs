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
    [SerializeField] Button cheatButton;
    public Button selectLevel;

    [Header("Audio")]
    [SerializeField] AudioSource selectAud;
    [SerializeField] AudioSource unlockAud;
    AudioSource inGameUnlockAud;

    [Header("Animations")]
    [SerializeField] Animator unlockAnim;

    [Header("Effects")]
    [SerializeField] ParticleSystem unlockedEffect;

    bool isUnlocked;
    SelectableWorlds parent;

    private void Start()
    {
        unlockedEffect.Pause();
        inGameUnlockAud = unlockAud;
    }
    private void OnEnable()
    {
        unlockedEffect.Pause();
/*        if (PlayerPrefs.HasKey("DevMode") && PlayerPrefs.GetInt("DevMode") == 1)
        {
            //cheatButton.enabled = true;
            cheatButton.gameObject.SetActive(true);
        }
        else
        {
            //cheatButton.enabled = false;
            cheatButton.gameObject.SetActive(false);
        }*/
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
    IEnumerator UnlockAnimation()
    {
        while (!parent.parentController.inPlace)
        {
            yield return null;
        }

        unlockedEffect.gameObject.SetActive(true);
        unlockAnim.enabled = true;
        unlockAnim.speed = 1;
        unlockedEffect.Play();
        if (inGameUnlockAud != null)
        {
            inGameUnlockAud.Play();
            inGameUnlockAud = null;
        }
        yield return new WaitForSeconds(0.5f);
        lockedImage.enabled = false;
    }
    IEnumerator LockAnimation()
    {
        while (!parent.parentController.inPlace)
        {
            yield return null;
        }
        lockedImage.enabled = true;
        unlockAnim.enabled = true;
        unlockAnim.Play("LevelUnlockSpiral", 0, 0);
        unlockAnim.speed = 0;
        yield return new WaitForSeconds(0f);
    }
    public void SetUnlocked(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
        if (isUnlocked)
        {
            if (this.isActiveAndEnabled)
            {
                StartCoroutine(UnlockAnimation());
            }
            while (!this.isActiveAndEnabled)
            {
                parent.parentController.nextWorld.onClick.Invoke();
                if (this.isActiveAndEnabled)
                {
                    StartCoroutine(UnlockAnimation());
                }
            }
        }
        else
        {
            if (this.isActiveAndEnabled)
            {
                lockedImage.gameObject.SetActive(true);
                StartCoroutine(LockAnimation());
            }
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
            PlayerPrefs.SetInt("SelectedWorld", parent.worldId);
            PlayerPrefs.SetInt("SelectedLevel", int.Parse(levelNumText.text));

            if (PlayerPrefs.HasKey("GameSpeed"))
            {
                Time.timeScale = PlayerPrefs.GetFloat("GameSpeed");
            }
            else
            {
                Time.timeScale = 1;
            }
            PlayerPrefs.SetInt("SelectedLevel", int.Parse(levelNumText.text));
            SceneManager.LoadScene(levelNameText.text);
        }
    }
}
