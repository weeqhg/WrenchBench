using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPause;

    public GameObject MenuSetting;
    public GameObject SettingOn;
    public GameObject SettingOff;
    private MainManager mainManager;
    public GameObject startGame;

    private void Start()
    {
        mainManager = gameObject.GetComponent<MainManager>();
        isPause = true;
    }
    private void Update()
    {
        if (!startGame.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (MenuSetting.activeSelf == true)
                {
                    MenuSetting.SetActive(false);
                    SettingOn.SetActive(true);
                    SettingOff.SetActive(false);
                    DeActivePause();
                }
                else
                {
                    MenuSetting.SetActive(true);
                    SettingOn.SetActive(false);
                    SettingOff.SetActive(true);
                    ActivePause();
                }
            }
        }
    }
    public void ActivePause()
    {
        mainManager.CameraMoveFromPlayer();
        isPause = true;
        //Debug.Log(isPause);
    }
    public void DeActivePause()
    {
        mainManager.CameraMoveOnPlayer();
        isPause = false;
        //Debug.Log(isPause);
    }
}
