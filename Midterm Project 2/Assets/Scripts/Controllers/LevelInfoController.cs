using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInfoController : MonoBehaviour
{
    [SerializeField] TMP_Text levelNumText;
    [SerializeField] TMP_Text levelNameText;
    [SerializeField] Image lockedImage;

    bool isUnlocked;

    public void SetNumber(int levelNumber)
    {
        levelNumText.text = levelNumber.ToString();
    }
    public void SetName(string levelName)
    {
        levelNameText.text = levelName;
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
        if (isUnlocked)
        {
            Debug.Log("Attempting to go to: " +  levelNameText.text); // Comment when levels are created
            //SceneManager.LoadScene(levelNameText.text); // Uncomment when levels are created
        }
    }
}
