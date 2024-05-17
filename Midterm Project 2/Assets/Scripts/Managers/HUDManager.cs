using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    [Header("----- Components -----")]
    [SerializeField] Image painSplash;
    [SerializeField] TMP_Text timerText;
    [SerializeField] Image staminaBar;
    public TMP_Text reticle;
    public static HUDManager instance;




    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var ts = TimeSpan.FromSeconds(GameManager.Instance.timer + 1);
        timerText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
    IEnumerator TakeDamage()
    {
        painSplash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        painSplash.gameObject.SetActive(false);
    }

    public void SetStamina(float current, float max)
    {
        staminaBar.fillAmount = current/max;
    }
}
