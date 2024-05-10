using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

[CreateAssetMenu]

public class SelectableWorlds : ScriptableObject
{
    [SerializeField] Image levelImagePrefab;
    public List<string> levelName;

    [Range(1,2)][SerializeField] float spacing;

    List<Image> levelImages;
    List<bool> levelUnlocks;
    Image currentImage;
    LevelInfoController levelInfoController;
    GameObject parent;
    float instantX;
    string fileName;

    void Awake()
    {
        fileName = ("/" + this.name.ToString() + ".dat");
        levelImages = new List<Image>();
        FileManager.instance.LoadLevelUnlocks(fileName);
        levelUnlocks = FileManager.instance.GetLevelUnlocks();

        while(levelUnlocks.Count < levelName.Count)
        {
            levelUnlocks.Add(false);
        }
        FileManager.instance.SaveLevelUnlocks(fileName);
        
        parent = GameObject.FindGameObjectWithTag("LevelSelect");
        instantX = -((levelName.Count * levelImagePrefab.rectTransform.sizeDelta.x) / 2 - (levelImagePrefab.rectTransform.sizeDelta.x / 2)) * spacing;
        
        for (int i = 0; i < levelName.Count; i++)
        {
            currentImage = Instantiate(levelImagePrefab, parent.transform);
            currentImage.transform.SetLocalPositionAndRotation(new Vector3(instantX, 0, 0), new Quaternion(0, 0, 0, 0));
            instantX += levelImagePrefab.rectTransform.sizeDelta.x * spacing;
            levelInfoController = currentImage.GetComponent<LevelInfoController>();
            levelInfoController.SetName(levelName[i]);
            levelInfoController.SetNumber(i + 1);
            levelInfoController.SetUnlocked(levelUnlocks[i]);
            levelImages.Add(currentImage);
        }
    }
    public void SetEnabled(bool isEnabled)
    {
        for (int i = 0; i < levelImages.Count; i++)
        {
            //Debug.Log("Disabling Level " + (i + 1).ToString());
            levelImages[i].gameObject.SetActive(isEnabled);
            //Debug.Log("Level " + (i + 1).ToString() + " Disabled");
        }
    }
    public void SaveLevelUnlocks()
    {
        for (int i = 0; i < levelImages.Count; i++)
        {
            //Debug.Log("Checking Level " + (i + 1).ToString());
            levelUnlocks[i] = levelImages[i].GetComponent<LevelInfoController>().GetUnlocked();
            //Debug.Log("Level " + (i + 1).ToString() + " Checked");
        }
        FileManager.instance.SetLevelUnlocks(levelUnlocks);
        FileManager.instance.SaveLevelUnlocks(fileName);
    }
}
