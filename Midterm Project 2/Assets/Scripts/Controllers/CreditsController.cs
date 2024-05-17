using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] List<TMP_Text> credits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }


        foreach (var credit in credits)
        {
            credit.transform.position = new Vector3(credit.transform.position.x, Mathf.Lerp(credit.transform.position.y, credit.transform.position.y + 10, Time.deltaTime * 10), credit.transform.position.z);
        }

        if (credits[0].transform.position.y > 3000)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}