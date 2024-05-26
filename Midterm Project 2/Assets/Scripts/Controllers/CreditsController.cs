using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] GameObject creditsObject;
    [SerializeField] List<TMP_Text> credits;
    [SerializeField] Button btnDefault;


    bool creditsActive;
    bool canCancel;

    Vector3 creditsInactiveLoc;
    // Start is called before the first frame update
    void Start()
    {
        creditsActive = false;
        canCancel = false;
        creditsInactiveLoc = creditsObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (creditsActive)
        {
            if (canCancel == true && Input.anyKeyDown)
            {
                creditsActive = false;
                canCancel = false;
                creditsObject.transform.position = creditsInactiveLoc;
                mainMenu.ToggleMainMenuActive();
            }

            creditsObject.transform.position = new Vector3(creditsObject.transform.position.x, Mathf.Lerp(creditsObject.transform.position.y, creditsObject.transform.position.y + 10, Time.deltaTime * 10), creditsObject.transform.position.z);

/*            foreach (var credit in credits)
            {
                //credit.transform.position = new Vector3(credit.transform.position.x, Mathf.Lerp(credit.transform.position.y, credit.transform.position.y + 10, Time.deltaTime * 10), credit.transform.position.z);
            }*/

            if (credits[0].transform.position.y > this.GetComponent<RectTransform>().rect.height + Screen.height * 2 + 500)
            {
                creditsActive = false;
                canCancel = false;
                creditsObject.transform.position = creditsInactiveLoc;
                mainMenu.ToggleMainMenuActive();
            }
        }
    }
    public void ToggleCreditsActive()
    {
        btnDefault.Select();
        creditsActive = !creditsActive;
        StartCoroutine(creditTimeMin());
    }
    IEnumerator creditTimeMin()
    {
        canCancel = false;
        yield return new WaitForSeconds(3f);
        canCancel = true;
    }
}