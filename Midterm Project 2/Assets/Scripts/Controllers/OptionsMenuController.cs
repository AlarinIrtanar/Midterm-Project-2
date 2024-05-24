using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject optionsMenu;

    bool optionsMenuActive;

    Vector3 optionsMenuActiveLoc;
    Vector3 optionsMenuInactiveLoc;

    [Header("----- Text -----")]
    [SerializeField] TMP_Text shootText;
    [SerializeField] TMP_Text grappleText;
    [SerializeField] TMP_Text crouchText;
    [SerializeField] TMP_Text sprintText;
    [SerializeField] TMP_Text jumpText;

    [Header("----- Sliders -----")]
    [SerializeField] Slider sensiSlider;
    [SerializeField] Slider speedSlider;

    bool shootPressed;
    bool grapplePressed;
    bool crouchPressed;
    bool sprintPressed;
    bool jumpPressed;

    private void Start()
    {
        optionsMenuActive = false;
        optionsMenuActiveLoc = optionsMenuInactiveLoc = optionsMenu.transform.position;

        optionsMenuInactiveLoc.y += Screen.height;
        optionsMenu.transform.position = optionsMenuInactiveLoc;
    }
    // Start is called before the first frame update
    void OnEnable()
    {

        // Shoot Button
        if(PlayerPrefs.HasKey("Shoot Button"))
        {
            shootText.text = PlayerPrefs.GetString("Shoot Button");
        }
        else
        {
            PlayerPrefs.SetString("Shoot Button", shootText.text);
        }

        // Grapple Button
        if (PlayerPrefs.HasKey("Grapple Button"))
        {
            grappleText.text = PlayerPrefs.GetString("Grapple Button");
        }
        else
        {
            PlayerPrefs.SetString("Grapple Button", grappleText.text);
        }

        // Crouch Button
        if (PlayerPrefs.HasKey("Crouch Button"))
        {
            crouchText.text = PlayerPrefs.GetString("Crouch Button");
        }
        else
        {
            PlayerPrefs.SetString("Crouch Button", crouchText.text);
        }

        // Sprint Button
        if (PlayerPrefs.HasKey("Sprint Button"))
        {
            sprintText.text = PlayerPrefs.GetString("Sprint Button");
        }
        else
        {
            PlayerPrefs.SetString("Sprint Button", sprintText.text);
        }

        // Jump Button
        if (PlayerPrefs.HasKey("Jump Button"))
        {
            jumpText.text = PlayerPrefs.GetString("Jump Button");
        }
        else
        {
            PlayerPrefs.SetString("Jump Button", jumpText.text);
        }

        // Sensitivity
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
           sensiSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            PlayerPrefs.SetFloat("Sensitivity", sensiSlider.value);
        }

        // Game Speed
        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            speedSlider.value = PlayerPrefs.GetFloat("GameSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("GameSpeed", speedSlider.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsMenuActive)
        {
            optionsMenu.transform.position = Vector3.Lerp(optionsMenu.transform.position, optionsMenuActiveLoc, Time.deltaTime * 5);
        }
        else
        {
            optionsMenu.transform.position = Vector3.Lerp(optionsMenu.transform.position, optionsMenuInactiveLoc, Time.deltaTime * 5);
        }

        if (shootPressed && Input.anyKeyDown)
        {
            string inputString = CheckInput(shootText.text);
            shootText.text = inputString;
            shootPressed = false;
        }
        if (grapplePressed && Input.anyKeyDown)
        {
            string inputString = CheckInput(grappleText.text);
            grappleText.text = inputString;
            grapplePressed = false;
        }
        if (crouchPressed && Input.anyKeyDown)
        {
            string inputString = CheckInput(crouchText.text);
            crouchText.text = inputString;
            crouchPressed = false;
        }
        if (sprintPressed && Input.anyKeyDown)
        {
            string inputString = CheckInput(sprintText.text);
            sprintText.text = inputString;
            sprintPressed = false;
        }
        if (jumpPressed && Input.anyKeyDown)
        {
            string inputString = CheckInput(jumpText.text);
            jumpText.text = inputString;
            jumpPressed = false;
        }
        /*        if (Input.GetKeyDown(shootText.text))
                {
                    Debug.Log("shootButton: " + shootText.text);
                }*/
    }
    public void Apply()
    {
        shootPressed = false;
        grapplePressed = false;
        crouchPressed = false;
        sprintPressed = false;
        jumpPressed = false;
        PlayerPrefs.SetString("Shoot Button", shootText.text);
        PlayerPrefs.SetString("Grapple Button", grappleText.text);
        PlayerPrefs.SetString("Crouch Button", crouchText.text);
        PlayerPrefs.SetString("Sprint Button", sprintText.text);
        PlayerPrefs.SetString("Jump Button", jumpText.text);

        PlayerPrefs.SetFloat("Sensitivity", sensiSlider.value);
        PlayerPrefs.SetFloat("GameSpeed", speedSlider.value);
        Time.timeScale = speedSlider.value;
    }
    public void ResetControls()
    {
        shootPressed = false;
        grapplePressed = false;
        crouchPressed = false;
        sprintPressed = false;
        jumpPressed = false;
        shootText.text = "mouse 0";
        grappleText.text = "mouse 1";
        crouchText.text = "left ctrl";
        sprintText.text = "left shift";
        jumpText.text = "space";

        sensiSlider.value = 1f;
        speedSlider.value = 1f;

        Apply();
    }

    public void PressShoot()
    {
        shootPressed = true;
    }
    public void PressGrapple()
    {
        grapplePressed = true;
    }
    public void PressCrouch()
    {
        crouchPressed = true;
    }
    public void PressSprint()
    {
        sprintPressed = true;
    }
    public void PressJump()
    {
        jumpPressed = true;
    }
    string CheckInput(string ogText)
    {
        string result = Input.inputString;

        if (result == "" || result == " ")
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                result = "down";
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                result = "up";
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                result = "left";
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                result = "right";
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                result = "mouse 0";
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                result = "mouse 1";
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                result = "left shift";
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                result = "right shift";
            }
            else if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                result = "left alt";
            }
            else if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                result = "right alt";
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                result = "left ctrl";
            }
            else if (Input.GetKeyDown(KeyCode.RightControl))
            {
                result = "right ctrl";
            }
            else if (Input.GetKeyDown(KeyCode.Insert))
            {
                result = "insert";
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                result = "delete";
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                result = "space";
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                result = "tab";
            }
            else
            {
                result = ogText;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            result = "backspace";
        }


        return result;
    }

    public void ToggleOptionsMenuActive()
    {
        optionsMenuActive = !optionsMenuActive;
    }
}
