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
        recordCurrent = PlayerPrefs.GetInt("РекордВолн", 0);
        recordWave.text = "Ваш рекорд: " + recordCurrent + " волн";
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
        money.text = "Руда: " + moneys;
        if (waveManager.countWave >= recordCurrent)
        {
            recordWave.text = "Ваш рекорд: " + waveManager.countWave + " волн";
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
            StartCoroutine(ChangeOrthoSize(10f, 4f, 1f)); // Плавно изменяем с 6 до 4 за 1 секунду
        }
    }

    private IEnumerator ChangeOrthoSize(float startSize, float endSize, float duration)
    {
        float elapsedTime = 0f;

        // Устанавливаем начальное значение
        virtualCamera.m_Lens.OrthographicSize = startSize;

        while (elapsedTime < duration)
        {
            // Интерполяция между начальным и конечным значением
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, endSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // Увеличиваем время
            yield return null; // Ждем следующий кадр
        }

        // Устанавливаем конечное значение
        virtualCamera.m_Lens.OrthographicSize = endSize;
    }
    public void CameraMoveFromPlayer()
    {
        if (virtualCamera != null)
        {
            StartCoroutine(ChangeOrthoSize(4f, 10f, 1f)); // Плавно изменяем с 6 до 4 за 1 секунду
        }
    }

    public void EndGame()
    {
        recordWaveEndMenu.text = "Ваш рекорд: " + waveManager.countWave + " волн";
        countMoneysEndGame.text = "Вы добыли: " + countMoneys + " руды";
        PlayerPrefs.SetInt("РекордВолн", waveManager.countWave);
    }

    public void RestartGame(string sceneName)
    {
        PlayerPrefs.SetInt("РекордВолн", waveManager.countWave);
        SceneManager.LoadSceneAsync(sceneName);
    }

}
