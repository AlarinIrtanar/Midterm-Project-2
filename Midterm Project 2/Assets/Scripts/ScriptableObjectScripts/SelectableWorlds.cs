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
    public string worldName;
    public List<string> levelName;
    public int worldId;

    [Range(1,2)][SerializeField] float spacing;

    public List<Image> levelImages;
    Image currentImage;
    LevelInfoController levelInfoController;
    GameObject parent;
    public LevelSelectController parentController;
    float instantX;
    bool firstLaunch = true;

    public void SetEnabled(bool isEnabled)
    {
        if (firstLaunch)
        {
            firstLaunch = false;
            levelImages = new List<Image>();

            parent = GameObject.FindGameObjectWithTag("LevelSelect");
            parentController = parent.GetComponent<LevelSelectController>();

            instantX = -((levelName.Count * levelImagePrefab.rectTransform.sizeDelta.x) / 2 - (levelImagePrefab.rectTransform.sizeDelta.x / 2)) * spacing;
            for (int i = 0; i < levelName.Count; i++)
            {
                currentImage = Instantiate(levelImagePrefab, parent.transform);
                currentImage.transform.SetLocalPositionAndRotation(new Vector3(instantX, 0, 0), new Quaternion(0, 0, 0, 0));
                instantX += levelImagePrefab.rectTransform.sizeDelta.x * spacing;
                levelInfoController = currentImage.GetComponent<LevelInfoController>();
                levelInfoController.SetParent(this);
                levelInfoController.SetName(levelName[i]);
                levelInfoController.SetNumber(i + 1);
                //levelInfoController.SetUnlocked(FileManager.instance.worlds[worldId].levels[i].isUnlocked);
                levelImages.Add(currentImage);
            }
        }
        for (int i = 0; i < levelImages.Count; i++)
        {
            //Debug.Log("Disabling Level " + (i + 1).ToString());
            levelImages[i].gameObject.SetActive(isEnabled);
            //Debug.Log("Level " + (i + 1).ToString() + " Disabled");
        }

    }

}
