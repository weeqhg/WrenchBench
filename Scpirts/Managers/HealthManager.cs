using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public Slider healthSliderFactory;
    public Text healthFactoryText;
    private PauseManager pauseManager;
    private MainManager mainManager;
    public GameObject endGame;

    private void Start()
    {
        endGame.SetActive(false);
        healthSliderFactory.interactable = false;
        healthSliderFactory.maxValue = 1f;
        healthSliderFactory.gameObject.SetActive(false);
        pauseManager = gameObject.GetComponent<PauseManager>();
        mainManager = gameObject.GetComponent<MainManager>();   
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (endGame.activeSelf)
        {
            pauseManager.isPause = true;
        }
    }
    public void UpdateHealthSlider()
    {
        healthSliderFactory.gameObject.SetActive(true);
        healthFactoryText.text = "Прочность: " + currentHealth + " из " + maxHealth;
        healthSliderFactory.value = (float)currentHealth / maxHealth;
        if (currentHealth < 0 && !pauseManager.isPause)
        {
            pauseManager.isPause = true;
            mainManager.EndGame();
            endGame.SetActive(true);
        }
    }
}
