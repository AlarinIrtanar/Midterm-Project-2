using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] GameObject creditsObject;
    [SerializeField] List<TMP_Text> credits;


    bool creditsActive;

    Vector3 creditsInactiveLoc;
    // Start is called before the first frame update
    void Start()
    {
        creditsActive = false;
        creditsInactiveLoc = creditsObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (creditsActive)
        {
            if (Input.anyKeyDown)
            {
                creditsActive = false;
                creditsObject.transform.position = creditsInactiveLoc;
                mainMenu.ToggleMainMenuActive();
            }

            creditsObject.transform.position = new Vector3(creditsObject.transform.position.x, Mathf.Lerp(creditsObject.transform.position.y, creditsObject.transform.position.y + 10, Time.deltaTime * 10), creditsObject.transform.position.z);

/*            foreach (var credit in credits)
            {
                //credit.transform.position = new Vector3(credit.transform.position.x, Mathf.Lerp(credit.transform.position.y, credit.transform.position.y + 10, Time.deltaTime * 10), credit.transform.position.z);
            }*/

            if (credits[0].transform.position.y > 1500)
            {
                creditsActive = false;
                creditsObject.transform.position = creditsInactiveLoc;
                mainMenu.ToggleMainMenuActive();
            }
        }
    }
    public void ToggleCreditsActive()
    {
        creditsActive = !creditsActive;
    }
}