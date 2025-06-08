using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Text money;
    public int moneys;
    public int countMoneys;
    public GameObject noHaveMoney;

    public GameObject BuildingDefenseMenu;
    public bool isActiveChangeDefense;

    public GameObject FactoryUpgradeMenu;
    public bool isActiveFactoryUpgradeMenu;

    public GameObject ShopMenu;
    public bool isActiveShop;

    public CinemachineVirtualCamera virtualCamera;
    public GameObject menuSetting;

    public Text recordWaveEndMenu;
    public Text countMoneysEndGame;

    public Text recordWave;
    private int recordCurrent;
    private WaveManager waveManager;

    public int repairForcePlayer;
    public int valueProduction;


    private PauseManager pauseManager;

    private void Start()
    {
        recordCurrent = PlayerPrefs.GetInt("����������", 0);
        recordWave.text = "��� ������: " + recordCurrent + " ����";
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.OrthographicSize = 10;
        }
        BuildingDefenseMenu.SetActive(false);
        noHaveMoney.SetActive(false);
        ShopMenu.SetActive(false);
        FactoryUpgradeMenu.SetActive(false);
        menuSetting.SetActive(false);
        waveManager = gameObject.GetComponent<WaveManager>();
        pauseManager = gameObject.GetComponent<PauseManager>();
    }
    private void Update()
    {
        money.text = "����: " + moneys;
        if (waveManager.countWave >= recordCurrent)
        {
            recordWave.text = "��� ������: " + waveManager.countWave + " ����";
        }
        CheckOpenMenu();
        if(pauseManager.isPause)
        {
            BuildingDefenseMenu.SetActive(false);
            FactoryUpgradeMenu.SetActive(false);
            ShopMenu.SetActive(false);
        }
    }
    private void CheckOpenMenu()
    {
        if (isActiveChangeDefense) BuildingDefenseMenu.SetActive(true);
        else BuildingDefenseMenu.SetActive(false);

        if (isActiveFactoryUpgradeMenu) FactoryUpgradeMenu.SetActive(true);
        else FactoryUpgradeMenu.SetActive(false);

        if (isActiveShop) ShopMenu.SetActive(true);
        else ShopMenu.SetActive(false);
    }


    public void CallMessagesNoHaveMoney()
    {
        StartCoroutine(MessageNoHaveMoney());
    }

    private IEnumerator MessageNoHaveMoney()
    {
        noHaveMoney.SetActive(true);
        yield return new WaitForSeconds(2);
        noHaveMoney.SetActive(false);
    }

    public void CameraMoveOnPlayer()
    {
        if (virtualCamera != null)
        {
            StartCoroutine(ChangeOrthoSize(10f, 4f, 1f)); // ������ �������� � 6 �� 4 �� 1 �������
        }
    }

    private IEnumerator ChangeOrthoSize(float startSize, float endSize, float duration)
    {
        float elapsedTime = 0f;

        // ������������� ��������� ��������
        virtualCamera.m_Lens.OrthographicSize = startSize;

        while (elapsedTime < duration)
        {
            // ������������ ����� ��������� � �������� ���������
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, endSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // ����������� �����
            yield return null; // ���� ��������� ����
        }

        // ������������� �������� ��������
        virtualCamera.m_Lens.OrthographicSize = endSize;
    }
    public void CameraMoveFromPlayer()
    {
        if (virtualCamera != null)
        {
            StartCoroutine(ChangeOrthoSize(4f, 10f, 1f)); // ������ �������� � 6 �� 4 �� 1 �������
        }
    }

    public void EndGame()
    {
        recordWaveEndMenu.text = "��� ������: " + waveManager.countWave + " ����";
        countMoneysEndGame.text = "�� ������: " + countMoneys + " ����";
        PlayerPrefs.SetInt("����������", waveManager.countWave);
    }

    public void RestartGame(string sceneName)
    {
        PlayerPrefs.SetInt("����������", waveManager.countWave);
        SceneManager.LoadSceneAsync(sceneName);
    }

}
